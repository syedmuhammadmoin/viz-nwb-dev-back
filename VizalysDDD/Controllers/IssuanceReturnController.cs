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
    public class IssuanceReturnController : ControllerBase
    {
        private readonly IIssuanceReturnService _issuanceReturnService;

        public IssuanceReturnController(IIssuanceReturnService issuanceReturnService)
        {
            _issuanceReturnService = issuanceReturnService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.IssuanceReturnClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<IssuanceReturnDto>>> CreateAsync(CreateIssuanceReturnDto entity)
        {
            var result = await _issuanceReturnService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.IssuanceReturnClaims.Create, Permissions.IssuanceReturnClaims.View, Permissions.IssuanceReturnClaims.Delete, Permissions.IssuanceReturnClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<IssuanceReturnDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var results = await _issuanceReturnService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.IssuanceReturnClaims.Create, Permissions.IssuanceReturnClaims.View, Permissions.IssuanceReturnClaims.Delete, Permissions.IssuanceReturnClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<IssuanceReturnDto>>> GetByIdAsync(int id)
        {
            var result = await _issuanceReturnService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.IssuanceReturnClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<IssuanceReturnDto>>> UpdateAsync(int id, CreateIssuanceReturnDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _issuanceReturnService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpPost("workflow")]
        public async Task<ActionResult<Response<bool>>> CheckWorkFlow([FromBody] ApprovalDto data)
        {
            var result = await _issuanceReturnService.CheckWorkFlow(data);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200
            return BadRequest(result);
        }
    }
}
