using MicroRabbit.Transfer.Application.Interfaces;

namespace MicroRabbit.Transfer.Console
{
    internal class TestData(ITransferService transferService)
    {
        private readonly ITransferService _transferService = transferService;

        public void PrintData()
        {
            var data = _transferService.GetTransferLogs().ToList();
            foreach (var item in data)
            {
                System.Console.WriteLine($"From: {item.FromAccount}, To: {item.ToAccount}, Amount: {item.TransferAmount}");
            }
        }
    }
}
