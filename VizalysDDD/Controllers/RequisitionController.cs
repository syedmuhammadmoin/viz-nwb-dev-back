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
    public class RequisitionController : ControllerBase
    {
        private readonly IRequisitionService _requisitionService;

        public RequisitionController(IRequisitionService requisitionService)
        {
            _requisitionService = requisitionService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.RequisitionClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<RequisitionDto>>> CreateAsync(CreateRequisitionDto entity)
        {
            var result = await _requisitionService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.RequisitionClaims.Create, Permissions.RequisitionClaims.View, Permissions.RequisitionClaims.Delete, Permissions.RequisitionClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<RequisitionDto>>>> GetAllAsync([FromQuery] PaginationFilter filter)
        {
            var results = await _requisitionService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.RequisitionClaims.View, Permissions.RequisitionClaims.Delete, Permissions.RequisitionClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<RequisitionDto>>> GetByIdAsync(int id)
        {
            var result = await _requisitionService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.RequisitionClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<RequisitionDto>>> UpdateAsync(int id, CreateRequisitionDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _requisitionService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.RequisitionClaims.View })]
        [HttpPost("workflow")]
        public async Task<ActionResult<Response<bool>>> CheckWorkFlow([FromBody] ApprovalDto data)
        {
            var result = await _requisitionService.CheckWorkFlow(data);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200
            return BadRequest(result);
        }
    }
}
