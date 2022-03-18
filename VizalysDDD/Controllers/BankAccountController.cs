using Application.Contracts.DTOs;
using Application.Contracts.Filters;
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
    public class BankAccountController : ControllerBase
    {
        private readonly IBankAccountService _bankAccountService;

        public BankAccountController(IBankAccountService bankAccountService)
        {
            _bankAccountService = bankAccountService;
        }
        [ClaimRequirement("Permission", new string[] { Permissions.BankAccountClaims.Create, Permissions.BankAccountClaims.View, Permissions.BankAccountClaims.Delete, Permissions.BankAccountClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<BankAccountDto>>>> GetAllAsync([FromQuery] PaginationFilter filter)
        {
            return Ok(await _bankAccountService.GetAllAsync(filter)); // Status Code : 200
        }

        [ClaimRequirement("Permission", new string[] { Permissions.BankAccountClaims.Create})]
        [HttpPost]
        public async Task<ActionResult<Response<BankAccountDto>>> CreateAsync(CreateBankAccountDto entity)
        {
            var bankAccount = await _bankAccountService.CreateAsync(entity);
            if (bankAccount.IsSuccess)
                return Ok(bankAccount); // Status Code : 200

            return BadRequest(bankAccount); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] {Permissions.BankAccountClaims.View, Permissions.BankAccountClaims.Delete, Permissions.BankAccountClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<BankAccountDto>>> GetByIdAsync(int id)
        {
            var result = await _bankAccountService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] {Permissions.BankAccountClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<BankAccountDto>>> UpdateAsync(int id, UpdateBankAccountDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _bankAccountService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpGet("Dropdown")]
        public async Task<ActionResult<Response<List<BankAccountDto>>>> GetBankAccountDropDown()
        {
            return Ok(await _bankAccountService.GetBankAccountDropDown()); // Status Code : 200
        }

    }
}
