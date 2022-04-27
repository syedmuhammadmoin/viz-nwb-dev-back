using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Helper;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Microsoft.AspNetCore.Authorization;
using Domain.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GRNController : ControllerBase
    {
        private readonly IGRNService _gRNService;

        public GRNController(IGRNService gRNService)
        {
            _gRNService = gRNService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.GRNClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<GRNDto>>> CreateAsync(CreateGRNDto entity)
        {
            var result = await _gRNService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.GRNClaims.Create, Permissions.GRNClaims.View, Permissions.GRNClaims.Delete, Permissions.GRNClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<GRNDto>>>> GetAllAsync([FromQuery] PaginationFilter filter)
        {
            var results = await _gRNService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.GRNClaims.View, Permissions.GRNClaims.Delete, Permissions.GRNClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<GRNDto>>> GetByIdAsync(int id)
        {
            var result = await _gRNService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.GRNClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<GRNDto>>> UpdateAsync(int id, CreateGRNDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _gRNService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.GRNClaims.View })]
        [HttpPost("workflow")]
        public async Task<ActionResult<Response<bool>>> CheckWorkFlow([FromBody] ApprovalDto data)
        {
            var result = await _gRNService.CheckWorkFlow(data);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200
            return BadRequest(result);
        }
    }
}
