using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Helper;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Domain.Constants;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepreciaitonAdjustmentController : ControllerBase
    {
        private readonly IDepreciationAdjustmentService _DepreciationAdjustmentService;

        public DepreciaitonAdjustmentController(IDepreciationAdjustmentService depreciationAdjustmentService)
        {
            _DepreciationAdjustmentService = depreciationAdjustmentService;
        }
        [ClaimRequirement("Permission", new string[] { Permissions.DepreciationAdjustmentClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<DepreciationAdjustmentDto>>> CreateAsync(CreateDepreciationAdjustmentDto entity)
        {
            var result = await _DepreciationAdjustmentService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.DepreciationAdjustmentClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<DepreciationAdjustmentDto>>> UpdateAsync(int id, CreateDepreciationAdjustmentDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _DepreciationAdjustmentService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.DepreciationAdjustmentClaims.Create, Permissions.DepreciationAdjustmentClaims.View, Permissions.DepreciationAdjustmentClaims.Delete, Permissions.DepreciationAdjustmentClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<DepreciationAdjustmentDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var results = await _DepreciationAdjustmentService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.DepreciationAdjustmentClaims.Create, Permissions.DepreciationAdjustmentClaims.View, Permissions.DepreciationAdjustmentClaims.Delete, Permissions.DepreciationAdjustmentClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<DepreciationAdjustmentDto>>> GetByIdAsync(int id)
        {
            var result = await _DepreciationAdjustmentService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }
        [HttpPost("workflow")]
        public async Task<ActionResult<Response<bool>>> CheckWorkFlow([FromBody] ApprovalDto data)
        {
            var result = await _DepreciationAdjustmentService.CheckWorkFlow(data);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200
            return BadRequest(result);
        }
    }
}
