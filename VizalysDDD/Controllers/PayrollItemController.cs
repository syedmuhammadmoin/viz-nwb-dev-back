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
    public class PayrollItemController : ControllerBase
    {
        private readonly IPayrollItemService _payrollItemService;

        public PayrollItemController(IPayrollItemService payrollItemService)
        {
            _payrollItemService = payrollItemService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.PayrollItemClaims.Create, Permissions.PayrollItemClaims.View, Permissions.PayrollItemClaims.Delete, Permissions.PayrollItemClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<PayrollItemDto>>>> GetAllAsync([FromQuery] PayrollItemFilter filter)
        {
            var results = await _payrollItemService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.PayrollItemClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<PayrollItemDto>>> CreateAsync(CreatePayrollItemDto entity)
        {
            var result = await _payrollItemService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.PayrollItemClaims.View, Permissions.PayrollItemClaims.Delete, Permissions.PayrollItemClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<PayrollItemDto>> GetByIdAsync(int id)
        {
            var result = await _payrollItemService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.PayrollItemClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<PayrollItemDto>>> UpdateAsync(int id, CreatePayrollItemDto entity)
        {
            if (id != entity.Id)
                return BadRequest("Id mismatch");

            var result = await _payrollItemService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.PayrollItemClaims.Create, Permissions.PayrollItemClaims.View, Permissions.PayrollItemClaims.Delete, Permissions.PayrollItemClaims.Edit })]
        [HttpGet("basicpays")]
        public async Task<ActionResult<PaginationResponse<List<PayrollItemDto>>>> GetBaicPayList()
        {
            var results = await _payrollItemService.GetBaicPayList();
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }

    }
}
