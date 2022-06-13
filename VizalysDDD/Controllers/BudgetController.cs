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
    public class BudgetController : ControllerBase
    {
        private readonly IBudgetService _budgetService;

        public BudgetController(IBudgetService budgetService)
        {
            _budgetService = budgetService;
        }
        [ClaimRequirement("Permission", new string[] { Permissions.BudgetClaims.Create, Permissions.BudgetClaims.View, Permissions.BudgetClaims.Delete, Permissions.BudgetClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<BudgetDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var budget = await _budgetService.GetAllAsync(filter);
            if (budget.IsSuccess)
                return Ok(budget); // Status Code : 200

            return BadRequest(budget); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.BudgetClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<BudgetDto>>> CreateAsync(CreateBudgetDto entity)
        {
            var budget = await _budgetService.CreateAsync(entity);
            if (budget.IsSuccess)
                return Ok(budget); // Status Code : 200

            return BadRequest(budget); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.BudgetClaims.Create, Permissions.BudgetClaims.View, Permissions.BudgetClaims.Delete, Permissions.BudgetClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<BudgetDto>>> GetByIdAsync(int id)
        {
            var result = await _budgetService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.BudgetClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<BudgetDto>>> UpdateAsync(int id, CreateBudgetDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _budgetService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpGet("Dropdown")]
        public async Task<ActionResult<Response<List<BudgetDto>>>> GetBudgetDropDown()
        {
            return Ok(await _budgetService.GetBudgetDropDown()); // Status Code : 200
        }

        [ClaimRequirement("Permission", new string[] { Permissions.BudgetReportClaims.View })]
        [HttpPost("BudgetReport")]
        public ActionResult<Response<List<BudgetReportDto>>> GetBudgetReport(BudgetReportFilters filters)
        {
            var budget = _budgetService.GetBudgetReport(filters);
            if (budget.IsSuccess)
                return Ok(budget); // Status Code : 200

            return BadRequest(budget); // Status code : 400
        }
    }
}
