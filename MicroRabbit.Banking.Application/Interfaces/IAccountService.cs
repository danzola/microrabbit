﻿using MicroRabbit.Banking.Application.Models;
using MicroRabbit.Banking.Domain.Models;

namespace MicroRabbit.Banking.Application.Interfaces
{
    public interface IAccountService
    {
        IEnumerable<Account> GetAccounts();
        Task Transfer(AccountTransfer accountTransfer);
        Task CancelAccount(string reason);
    }
}
