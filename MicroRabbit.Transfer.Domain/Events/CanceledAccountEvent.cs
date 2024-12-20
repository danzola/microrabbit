﻿using MicroRabbit.Domain.Core.Events;

namespace MicroRabbit.Transfer.Domain.Events
{
    public class CanceledAccountEvent(string reason) : Event
    {
        public string Reason { get; private set; } = reason;
    }
}
