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
    public class IssuanceController : ControllerBase
    {
        private readonly IIssuanceService _issuance;

        public IssuanceController(IIssuanceService issuance)
        {
            _issuance = issuance;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.IssuanceClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<IssuanceDto>>> CreateAsync(CreateIssuanceDto entity)
        {
            var result = await _issuance.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.IssuanceClaims.Create, Permissions.IssuanceClaims.View, Permissions.IssuanceClaims.Delete, Permissions.IssuanceClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<IssuanceDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var results = await _issuance.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.IssuanceClaims.Create, Permissions.IssuanceClaims.View, Permissions.IssuanceClaims.Delete, Permissions.IssuanceClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<IssuanceDto>>> GetByIdAsync(int id)
        {
            var result = await _issuance.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.IssuanceClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<IssuanceDto>>> UpdateAsync(int id, CreateIssuanceDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _issuance.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpPost("workflow")]
        public async Task<ActionResult<Response<bool>>> CheckWorkFlow([FromBody] ApprovalDto data)
        {
            var result = await _issuance.CheckWorkFlow(data);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200
            return BadRequest(result);
        }

    }
}
