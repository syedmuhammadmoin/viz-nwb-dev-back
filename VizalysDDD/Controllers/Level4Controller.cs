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
    public class Level4Controller : ControllerBase
    {
        private readonly ILevel4Service _level4Service;

        public Level4Controller(ILevel4Service level4Service)
        {
            _level4Service = level4Service;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.Level4Claims.Create, Permissions.Level4Claims.View, Permissions.Level4Claims.Delete, Permissions.Level4Claims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<Level4Dto>>>> GetAllAsync([FromQuery] Level4Filter filter)
        {
            var level4 = await _level4Service.GetAllAsync(filter);
            if (level4.IsSuccess)
                return Ok(level4); // Status Code : 200

            return BadRequest(level4); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.Level4Claims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<Level4Dto>>> CreateAsync(CreateLevel4Dto entity)
        {
            var level4 = await _level4Service.CreateAsync(entity);
            if (level4.IsSuccess)
                return Ok(level4); // Status Code : 200

            return BadRequest(level4); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.Level4Claims.Create, Permissions.Level4Claims.View, Permissions.Level4Claims.Delete, Permissions.Level4Claims.Edit })]
        [HttpGet("{id}")]
        public async Task<ActionResult<Response<Level4Dto>>> GetByIdAsync(string id)
        {
            var result = await _level4Service.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.Level4Claims.Edit })]
        [HttpPut("{id}")]
        public async Task<ActionResult<Response<Level4Dto>>> UpdateAsync(string id, CreateLevel4Dto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _level4Service.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpGet("Dropdown")]
        public async Task<ActionResult<Response<List<Level4Dto>>>> GetLevel4DropDown()
        {
            return Ok(await _level4Service.GetLevel4DropDown()); // Status Code : 200
        }

        [HttpGet("OtherAccounts")]
        public async Task<ActionResult<Response<List<Level4Dto>>>> GetAllOtherAccounts()
        {
            return Ok(await _level4Service.GetAllOtherAccounts()); // Status Code : 200
        }
        [HttpGet("budgetAccounts")]
        public async Task<ActionResult<Response<List<Level4Dto>>>> GetBudgetAccounts()
        {
            return Ok(await _level4Service.GetBudgetAccounts()); // Status Code : 200
        }

        [HttpGet("payables")]
        public async Task<ActionResult<Response<List<Level4Dto>>>> GetPayableAccounts()
        {
            return Ok(await _level4Service.GetPayableAccounts()); // Status Code : 200
        }

        [HttpGet("receivables")]
        public async Task<ActionResult<Response<List<Level4Dto>>>> GetReceivableAccounts()
        {
            return Ok(await _level4Service.GetReceivableAccounts()); // Status Code : 200
        }

        [HttpGet("nonCurrentAsset")]
        public async Task<ActionResult<Response<List<Level4Dto>>>> GetNonCurrentAssetAccounts()
        {
            return Ok(await _level4Service.GetNonCurrentAssetAccounts()); // Status Code : 200
        }

        [HttpGet("nonCurrentLiabilities")]
        public async Task<ActionResult<Response<List<Level4Dto>>>> GetNonCurrentLiabilitiesAccounts()
        {
            return Ok(await _level4Service.GetNonCurrentLiabilitiesAccounts()); // Status Code : 200
        }

        [HttpGet("expenseAccounts")]
        public async Task<ActionResult<Response<List<Level4Dto>>>> GetExpenseAccounts()
        {
            return Ok(await _level4Service.GetExpenseAccounts()); // Status Code : 200
        }
        [HttpGet("incomeAccounts")]
        public async Task<ActionResult<Response<List<Level4Dto>>>> GetIncomeAccounts()
        {
            return Ok(await _level4Service.GetIncomeAccounts()); // Status Code : 200
        }
        [HttpGet("CashBankAccounts")]
        public async Task<ActionResult<Response<List<Level4Dto>>>> GetCashBankAccounts()
        {
            return Ok(await _level4Service.GetCashBankAccounts());
        }

        [HttpGet("CurrentAssetAccounts")]
        public async Task<ActionResult<Response<List<Level4Dto>>>> GetCurrentAssetAccounts()
        {
            return Ok(await _level4Service.GetCurrentAssetAccounts());
        }
    }
}
