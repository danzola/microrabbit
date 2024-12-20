using MediatR;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Banking.Domain.Events;
using MicroRabbit.Domain.Core.Bus;

namespace MicroRabbit.Banking.Domain.CommandHandlers
{
    public class CancelAccountCommandHandler(IEventBus eventBus) : IRequestHandler<CancelAccountCommand, bool>
    {
        private readonly IEventBus _eventBus = eventBus;
        public async Task<bool> Handle(CancelAccountCommand request, CancellationToken cancellationToken)
        {
            await _eventBus.Publish(new CanceledAccountEvent(request.Reason));
            return true;
        }
    }
}
