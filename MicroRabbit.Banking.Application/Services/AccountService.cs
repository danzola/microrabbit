using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Models;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Banking.Domain.Models;
using MicroRabbit.Domain.Core.Bus;

namespace MicroRabbit.Banking.Application.Services
{
    public class AccountService(IAccountRepository accountRepository, IEventBus eventBus) : IAccountService
    {
        private readonly IAccountRepository _accountRepository = accountRepository;
        private readonly IEventBus _eventBus = eventBus;

        public async Task CancelAccount(string reason)
        {
            CancelAccountCommand cancelAccountCommand = new(reason);
            await _eventBus.SendCommand(cancelAccountCommand);
        }

        public IEnumerable<Account> GetAccounts()
        {
            return _accountRepository.GetAccounts();
        }

        public async Task Transfer(AccountTransfer accountTransfer)
        {
            CreateTransferCommand createTransferCommand = new(
                accountTransfer.FromAccount,
                accountTransfer.ToAccount,
                accountTransfer.TransferAmount);

            await _eventBus.SendCommand(createTransferCommand);
        }
    }
}
