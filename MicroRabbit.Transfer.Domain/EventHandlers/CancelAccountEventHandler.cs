using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Transfer.Domain.Events;

namespace MicroRabbit.Transfer.Domain.EventHandlers
{
    public class CancelAccountEventHandler : IEventHandler<CanceledAccountEvent>
    {
        public Task Handle(CanceledAccountEvent @event)
        {
            Console.WriteLine($"Account Canceled: Reason: {@event.Reason}");
            return Task.CompletedTask;
        }
    }
}
