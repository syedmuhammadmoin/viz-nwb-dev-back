using Application.Contracts.DTOs;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankReconController : ControllerBase
    {
        private readonly IBankReconService _bankReconService;

        public BankReconController(IBankReconService bankReconService)
        {
            _bankReconService = bankReconService;
        }
        [HttpPost]
        public async Task<ActionResult<Response<int>>> AddBankReconcile(CreateBankReconDto[] entity)
        {
            var bankRecon = await _bankReconService.CreateAsync(entity);
            if (bankRecon.IsSuccess)
                return Ok(bankRecon); // Status Code : 200

            return BadRequest(bankRecon); // Status code : 400
        }
    }
}
