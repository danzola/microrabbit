﻿using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Transfer.Domain.Events;
using MicroRabbit.Transfer.Domain.Interfaces;
using MicroRabbit.Transfer.Domain.Models;

namespace MicroRabbit.Transfer.Domain.EventHandlers
{
    public class TransferEventHandler(ITransferLogRepository transferLogRepository) : IEventHandler<TransferCreatedEvent>
    {
        private readonly ITransferLogRepository _transferLogRepository = transferLogRepository;

        public Task Handle(TransferCreatedEvent @event)
        {
            Console.WriteLine($"Transfer Event Handled: From: {@event.From} To: {@event.To} Amount: {@event.Amount}");
            return _transferLogRepository.Add(new TransferLog
            {
                FromAccount = @event.From,
                ToAccount = @event.To,
                TransferAmount = @event.Amount
            });
        }
    }
}
