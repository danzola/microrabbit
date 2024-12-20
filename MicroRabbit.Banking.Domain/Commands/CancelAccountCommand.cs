using MicroRabbit.Domain.Core.Commands;

namespace MicroRabbit.Banking.Domain.Commands
{
    public class CancelAccountCommand(string reason) : Command
    {
        public string Reason { get; protected set; } = reason;
    }
}
