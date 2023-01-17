using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Helper;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Application.Services;
using Domain.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepreciationController : ControllerBase
    {
        private readonly IDepreciationService _depreciationService;

        public DepreciationController(IDepreciationService depreciationService)
        {
            _depreciationService = depreciationService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.DepreciationClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<DepreciationDto>>> CreateAsync(CreateDepreciationDto entity)
        {
            var result = await _depreciationService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.DepreciationClaims.Create, Permissions.DepreciationClaims.View, Permissions.DepreciationClaims.Delete, Permissions.DepreciationClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<DepreciationDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var results = await _depreciationService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }
        [ClaimRequirement("Permission", new string[] { Permissions.DepreciationClaims.Create, Permissions.DepreciationClaims.View, Permissions.DepreciationClaims.Delete, Permissions.DepreciationClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<DepreciationDto>>> GetByIdAsync(int id)
        {
            var result = await _depreciationService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }
        [ClaimRequirement("Permission", new string[] { Permissions.DepreciationClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<DepreciationDto>>> UpdateAsync(int id, CreateDepreciationDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _depreciationService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }
    }
}
