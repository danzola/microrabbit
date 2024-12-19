using MicroRabbit.Transfer.Application.Interfaces;
using MicroRabbit.Transfer.Domain.Interfaces;
using MicroRabbit.Transfer.Domain.Models;

namespace MicroRabbit.Transfer.Application.Services
{
    public class TransferService(ITransferLogRepository transferLogRepository) : ITransferService
    {
        private readonly ITransferLogRepository transferLogRepository = transferLogRepository;
        public IEnumerable<TransferLog> GetTransferLogs()
        {
            return transferLogRepository.GetTransferLogs(); 
        }
    }
}
