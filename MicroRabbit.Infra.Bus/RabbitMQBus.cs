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
    public sealed class RabbitMQBus(IMediator mediator, IServiceScopeFactory serviceScopeFactory) : IEventBus
    {
        private readonly IMediator _mediator = mediator;
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
        private readonly Dictionary<string, List<Type>> _handlers = [];
        private readonly List<Type> _eventTypes = [];

        public Task SendCommand<T>(T command) where T : Command
        {
            return _mediator.Send(command);
        }

        public async Task Publish<T>(T @event) where T : Event
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            var eventName = @event.GetType().Name;

            channel.QueueDeclare(eventName, false, false, false, null);

            var message = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish("", eventName, null, body);
        }

        public async Task Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler<T>
        {
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

            StartBasicConsume<T>();
        }

        private void StartBasicConsume<T>() where T : Event
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                DispatchConsumersAsync = true
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            var eventName = typeof(T).Name;

            channel.QueueDeclare(eventName, false, false, false, null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += Consumer_ReceivedAsync;

            channel.BasicConsume(eventName, true, consumer);
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
                    var @event = JsonConvert.DeserializeObject(message, eventType);
                    var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
                    await (Task)concreteType.GetMethod("Handle")?.Invoke(handler, [@event]);
                }
            }
        }
    }
}
