using MicroRabbit.Transfer.Application.Interfaces;
using MicroRabbit.Transfer.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace MicroRabbit.Transfer.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController(ITransferService transferService) : ControllerBase
    {
        private readonly ITransferService _transferService = transferService;

        [HttpGet]
        public IEnumerable<TransferLog> Get()
        {
            return _transferService.GetTransferLogs();
        }
    }
}
