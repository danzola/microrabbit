using MicroRabbit.Transfer.Domain.Models;

namespace MicroRabbit.Transfer.Domain.Interfaces
{
    public interface ITransferLogRepository
    {
        IEnumerable<TransferLog> GetTransferLogs();
    }
}
