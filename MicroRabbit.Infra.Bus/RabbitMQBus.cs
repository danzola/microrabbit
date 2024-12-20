using System.Text;
using MediatR;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Commands;
using MicroRabbit.Domain.Core.Events;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MicroRabbit.Infra.Bus
{
    public sealed class RabbitMQBus : IEventBus, IAsyncDisposable
    {
        private readonly IMediator _mediator;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly Dictionary<string, List<Type>> _handlers = [];
        private readonly List<Type> _eventTypes = [];
        private IConnection? _connection;
        private IChannel? _channel;

        public RabbitMQBus(IMediator mediator, IServiceScopeFactory serviceScopeFactory)
        {
            _mediator = mediator;
            _serviceScopeFactory = serviceScopeFactory;
        }

        private async Task InitializeConnectionAsync()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            _connection = await factory.CreateConnectionAsync();
            _channel = await _connection.CreateChannelAsync();
        }

        public Task SendCommand<T>(T command) where T : Command
        {
            return _mediator.Send(command);
        }

        public async Task Publish<T>(T @event) where T : Event
        {
            if (_connection is null || _channel is null)
            {
                await InitializeConnectionAsync();
            }
            
            var eventName = @event.GetType().Name;

            await _channel!.QueueDeclareAsync(eventName, false, false, false, null);

            var message = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(message);

            await _channel.BasicPublishAsync(exchange: "", routingKey: eventName, body: body);            
        }

        public async Task Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler<T>
        {
            if (_connection is null || _channel is null)
            {
                await InitializeConnectionAsync();
            }

            var eventName = typeof(T).Name;
            var handlerType = typeof(TH);

            if (!_eventTypes.Contains(typeof(T)))
            {
                _eventTypes.Add(typeof(T));
            }

            if (!_handlers.ContainsKey(eventName))
            {
                _handlers.Add(eventName, new List<Type>());
            }

            if (_handlers[eventName].Any(s => s.GetType() == handlerType))
            {
                throw new ArgumentException(
                    $"Handler Type {handlerType.Name} already is registered for '{eventName}'", nameof(handlerType));
            }

            _handlers[eventName].Add(handlerType);

            await StartBasicConsumeAsync<T>();
        }

        private async Task StartBasicConsumeAsync<T>() where T : Event
        {
            var eventName = typeof(T).Name;

            await _channel!.QueueDeclareAsync(
                queue: eventName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += Consumer_ReceivedAsync;

            await _channel.BasicConsumeAsync(
                queue: eventName,
                autoAck: true,
                consumer: consumer);
        }

        private async Task Consumer_ReceivedAsync(object sender, BasicDeliverEventArgs e)
        {
            var eventName = e.RoutingKey;
            var body = @e.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            try
            {
                await ProcessEvent(eventName,message).ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error consuming message: {ex.Message}");
                throw;
            }
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            if(_handlers.TryGetValue(eventName, out List<Type>? subscriptions))
            {
                using var scope = _serviceScopeFactory.CreateScope();
                foreach (var subscription in subscriptions)
                {
                    var handler = scope.ServiceProvider.GetService(subscription);
                    if (handler == null) continue;

                    var eventType = _eventTypes.SingleOrDefault(t => t.Name == eventName);
                    if (eventType == null) continue;

                    var @event = JsonConvert.DeserializeObject(message, eventType);
                    var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
                    await (Task)concreteType.GetMethod("Handle")?.Invoke(handler, [@event]);
                }
            }
        }
        public async ValueTask DisposeAsync()
        {
            if (_channel?.IsOpen == true)
                await _channel.CloseAsync();

            if (_connection?.IsOpen == true)
                await _connection.CloseAsync();

            GC.SuppressFinalize(this);
        }

    }
}
