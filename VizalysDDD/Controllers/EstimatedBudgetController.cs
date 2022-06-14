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
    public class EstimatedBudgetController : ControllerBase
    {
        private readonly IEstimatedBudgetService _budgetService;

        public EstimatedBudgetController(IEstimatedBudgetService budgetService)
        {
            _budgetService = budgetService;
        }
        [ClaimRequirement("Permission", new string[] { Permissions.EstimatedBudgetClaims.Create, Permissions.EstimatedBudgetClaims.View, Permissions.EstimatedBudgetClaims.Delete, Permissions.EstimatedBudgetClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<EstimatedBudgetDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var budget = await _budgetService.GetAllAsync(filter);
            if (budget.IsSuccess)
                return Ok(budget); // Status Code : 200

            return BadRequest(budget); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.EstimatedBudgetClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<EstimatedBudgetDto>>> CreateAsync(CreateEstimatedBudgetDto entity)
        {
            var budget = await _budgetService.CreateAsync(entity);
            if (budget.IsSuccess)
                return Ok(budget); // Status Code : 200

            return BadRequest(budget); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.EstimatedBudgetClaims.Create, Permissions.EstimatedBudgetClaims.View, Permissions.EstimatedBudgetClaims.Delete, Permissions.EstimatedBudgetClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<EstimatedBudgetDto>>> GetByIdAsync(int id)
        {
            var result = await _budgetService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.EstimatedBudgetClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<EstimatedBudgetDto>>> UpdateAsync(int id, CreateEstimatedBudgetDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _budgetService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpGet("Dropdown")]
        public async Task<ActionResult<Response<List<EstimatedBudgetDto>>>> GetEstimatedBudgetDropDown()
        {
            return Ok(await _budgetService.GetEstimatedBudgetDropDown()); // Status Code : 200
        }
    }
}
