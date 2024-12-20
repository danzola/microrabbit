using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Transfer.Domain.Interfaces;
using MicroRabbit.Transfer.Domain.Models;

namespace MicroRabbit.Transfer.Data.Repository
{
    public class TransferLogRepository(TransferDbContext context) : ITransferLogRepository
    {
        private readonly TransferDbContext _context = context;

        public IEnumerable<TransferLog> GetTransferLogs()
        {
            return _context.TransferLogs;
        }

        public Task Add(TransferLog transferLog)
        {
            _context.TransferLogs.Add(transferLog);
            return _context.SaveChangesAsync();
        }
    }
}
