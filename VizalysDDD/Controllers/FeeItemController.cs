using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Helper;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Domain.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeeItemController : ControllerBase
    {
        private readonly IFeeItemService _feeItemService;

        public FeeItemController(IFeeItemService feeItemService)
        {
            _feeItemService = feeItemService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.FeeItemClaims.Create, Permissions.FeeItemClaims.View, Permissions.FeeItemClaims.Delete, Permissions.FeeItemClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<FeeItemDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var results = await _feeItemService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.FeeItemClaims.Create, Permissions.FeeItemClaims.View, Permissions.FeeItemClaims.Delete, Permissions.FeeItemClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<FeeItemDto>> GetByIdAsync(int id)
        {
            var result = await _feeItemService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpGet("Dropdown")]
        public async Task<ActionResult<Response<List<FeeItemDto>>>> GetDropDown()
        {
            var result = await _feeItemService.GetDropDown();
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.FeeItemClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<FeeItemDto>>> CreateAsync(CreateFeeItemDto entity)
        {
            var result = await _feeItemService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.FeeItemClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<FeeItemDto>>> UpdateAsync(int id, CreateFeeItemDto entity)
        {
            if (id != entity.Id)
                return BadRequest("Id mismatch");

            var result = await _feeItemService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

    }
}
