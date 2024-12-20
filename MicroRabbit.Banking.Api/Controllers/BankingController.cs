using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Models;
using MicroRabbit.Banking.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace MicroRabbit.Banking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankingController(IAccountService accountService) : ControllerBase
    {
        private readonly IAccountService _accountService = accountService;

        [HttpGet]
        public ActionResult<IEnumerable<Account>> Get()
        {
            return Ok(_accountService.GetAccounts());
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AccountTransfer accountTransfer)
        {
            await _accountService.Transfer(accountTransfer);
            return Ok(accountTransfer);
        }

        [HttpPost("Cancel")]
        public async Task<IActionResult> CancelAccount([FromBody] string reason)
        {
            await _accountService.CancelAccount(reason);
            return Ok();
        }
    }
}
