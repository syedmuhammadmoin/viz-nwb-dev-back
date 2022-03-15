using Application.Contracts.DTOs;
using Application.Contracts.Helper;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BankReconController : ControllerBase
    {
        private readonly IBankReconService _bankReconService;

        public BankReconController(IBankReconService bankReconService)
        {
            _bankReconService = bankReconService;
        }
        
        [ClaimRequirement("Permission", new string[] { Permissions.BankReconClaims.Create })]
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
