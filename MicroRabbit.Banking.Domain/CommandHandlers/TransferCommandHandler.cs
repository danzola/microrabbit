using MediatR;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Banking.Domain.Events;
using MicroRabbit.Domain.Core.Bus;

namespace MicroRabbit.Banking.Domain.CommandHandlers
{
    public class TransferCommandHandler(IEventBus eventBus) : IRequestHandler<CreateTransferCommand, bool>
    {
        private readonly IEventBus _eventBus = eventBus;

        public async Task<bool> Handle(CreateTransferCommand request, CancellationToken cancellationToken)
        {
            await _eventBus.Publish(new TransferCreatedEvent(request.From, request.To, request.Amount));
            return true;
        }
    }
}
