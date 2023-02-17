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
    public class DepreciationModelController : ControllerBase
    {
        private readonly IDepreciationModelService _depreciationService;

        public DepreciationModelController(IDepreciationModelService depreciationService)
        {
            _depreciationService = depreciationService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.DepreciationModelClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<DepreciationModelDto>>> CreateAsync(CreateDepreciationModelDto entity)
        {
            var result = await _depreciationService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.DepreciationModelClaims.Create, Permissions.DepreciationModelClaims.View, Permissions.DepreciationModelClaims.Delete, Permissions.DepreciationModelClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<DepreciationModelDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var results = await _depreciationService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }
        [ClaimRequirement("Permission", new string[] { Permissions.DepreciationModelClaims.Create, Permissions.DepreciationModelClaims.View, Permissions.DepreciationModelClaims.Delete, Permissions.DepreciationModelClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<DepreciationModelDto>>> GetByIdAsync(int id)
        {
            var result = await _depreciationService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }
        [ClaimRequirement("Permission", new string[] { Permissions.DepreciationModelClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<DepreciationModelDto>>> UpdateAsync(int id, CreateDepreciationModelDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _depreciationService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpGet("Dropdown")]
        public async Task<ActionResult<Response<List<CategoryDto>>>> GetCategoryDropDown()
        {
            return Ok(await _depreciationService.GetDepreciationDown()); // Status Code : 200
        }
    }
}
