using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Transfer.Domain.Events;

namespace MicroRabbit.Transfer.Domain.EventHandlers
{
    public class TransferEventHandler : IEventHandler<TransferCreatedEvent>
    {

        public Task Handle(TransferCreatedEvent @event)
        {
            Console.WriteLine($"Transfer Event Handled: From: {@event.From} To: {@event.To} Amount: {@event.Amount}");
            return Task.CompletedTask;
        }
    }
}
