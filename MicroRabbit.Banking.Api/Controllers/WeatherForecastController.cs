using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace MicroRabbit.Banking.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController(ILogger<WeatherForecastController> logger, IAccountService accountService) : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger = logger;
        private readonly IAccountService _accountService = accountService;

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<Account> Get()
        {
            return _accountService.GetAccounts();
        }
    }
}
