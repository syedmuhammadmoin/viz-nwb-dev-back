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
    public class FixedAssetController : ControllerBase
    {
        private readonly IFixedAssetService _fixedAssetService;

        public FixedAssetController(IFixedAssetService fixedAssetService)
        {
            _fixedAssetService = fixedAssetService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.FixedAssetClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<FixedAssetDto>>> CreateAsync(CreateFixedAssetDto entity)
        {
            var result = await _fixedAssetService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.FixedAssetClaims.Create, Permissions.FixedAssetClaims.View, Permissions.FixedAssetClaims.Delete, Permissions.FixedAssetClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<FixedAssetDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var results = await _fixedAssetService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }
       
        [ClaimRequirement("Permission", new string[] { Permissions.FixedAssetClaims.Create, Permissions.FixedAssetClaims.View, Permissions.FixedAssetClaims.Delete, Permissions.FixedAssetClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<FixedAssetDto>>> GetByIdAsync(int id)
        {
            var result = await _fixedAssetService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }
        
        [ClaimRequirement("Permission", new string[] { Permissions.FixedAssetClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<FixedAssetDto>>> UpdateAsync(int id, CreateFixedAssetDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _fixedAssetService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpPost("workflow")]
        public async Task<ActionResult<Response<bool>>> CheckWorkFlow([FromBody] ApprovalDto data)
        {
            var result = await _fixedAssetService.CheckWorkFlow(data);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status Code : 400
        }
        [HttpGet("Dropdown")]
        public async Task<ActionResult<Response<List<Level4Dto>>>> GetFixedAssetDropDown()
        {
            return Ok(await _fixedAssetService.GetAssetDropDown()); // Status Code : 200
        }
        [HttpGet("Disposable/Dropdown")]
        public async Task<ActionResult<Response<List<Level4Dto>>>> GetDisposableAssetDropDown()
        {
            return Ok(await _fixedAssetService.GetDisposableAssetDropDown()); // Status Code : 200
        }
        [HttpGet("Product/{ProductId:int}")]
        public async Task<ActionResult<Response<List<Level4Dto>>>> GetAssetByProductIdDropDown(int ProductId)
        {
            return Ok(await _fixedAssetService.GetAssetByProductIdDropDown(ProductId)); // Status Code : 200
        }
        
    }
}
