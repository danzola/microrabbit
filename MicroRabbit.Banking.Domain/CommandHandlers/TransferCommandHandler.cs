using MediatR;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Domain.Core.Bus;

namespace MicroRabbit.Banking.Domain.CommandHandlers
{
    public class TransferCommandHandler(IEventBus eventBus) : IRequestHandler<CreateTransferCommand, bool>
    {
        private readonly IEventBus _eventBus = eventBus;

        public Task<bool> Handle(CreateTransferCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
