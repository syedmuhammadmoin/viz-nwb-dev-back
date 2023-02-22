using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Helper;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Domain.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetReappropriationController : ControllerBase
    {
        private readonly IBudgetReappropriationService _budgetReappropriationService;

        public BudgetReappropriationController(IBudgetReappropriationService budgetReappropriationService)
        {
            _budgetReappropriationService = budgetReappropriationService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.BudgetReappropriationClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<BudgetReappropriationDto>>> CreateAsync(CreateBudgetReappropriationDto entity)
        {
            var budgetReappropriationService = await _budgetReappropriationService.CreateAsync(entity);
            if (budgetReappropriationService.IsSuccess)
                return Ok(budgetReappropriationService); // Status Code : 200

            return BadRequest(budgetReappropriationService); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.BudgetReappropriationClaims.Create, Permissions.BudgetReappropriationClaims.View, Permissions.BudgetReappropriationClaims.Delete, Permissions.BudgetReappropriationClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<BudgetReappropriationDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var bills = await _budgetReappropriationService.GetAllAsync(filter);
            if (bills.IsSuccess)
                return Ok(bills); // Status Code : 200

            return BadRequest(bills); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.BudgetReappropriationClaims.Create, Permissions.BudgetReappropriationClaims.View, Permissions.BudgetReappropriationClaims.Delete, Permissions.BudgetReappropriationClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<BudgetReappropriationDto>>> GetByIdAsync(int id)
        {
            var result = await _budgetReappropriationService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.BudgetReappropriationClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<BudgetReappropriationDto>>> UpdateAsync(int id, CreateBudgetReappropriationDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _budgetReappropriationService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpPost("workflow")]
        public async Task<ActionResult<Response<bool>>> CheckWorkFlow([FromBody] ApprovalDto data)
        {
            var result = await _budgetReappropriationService.CheckWorkFlow(data);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200
            return BadRequest(result);
        }

    }
}
