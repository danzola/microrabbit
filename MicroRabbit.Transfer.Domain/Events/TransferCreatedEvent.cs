using MicroRabbit.Domain.Core.Events;

namespace MicroRabbit.Transfer.Domain.Events
{
    public class TransferCreatedEvent(int from, int to, decimal amount) : Event
    {
        public int From { get; private set; } = from;
        public int To { get; private set; } = to;
        public decimal Amount { get; private set; } = amount;
    }
}
