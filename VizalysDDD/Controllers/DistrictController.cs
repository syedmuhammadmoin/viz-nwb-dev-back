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
    public class DistrictController : ControllerBase
    {
        private readonly IDistrictService _districtService;

        public DistrictController(IDistrictService districtService)
        {
            _districtService = districtService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.DistrictClaims.Create, Permissions.DistrictClaims.View, Permissions.DistrictClaims.Delete, Permissions.DistrictClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<DistrictDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var results = await _districtService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.DistrictClaims.Create, Permissions.DistrictClaims.View, Permissions.DistrictClaims.Delete, Permissions.DistrictClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<DistrictDto>> GetByIdAsync(int id)
        {
            var result = await _districtService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpGet("Dropdown")]
        public async Task<ActionResult<Response<List<DistrictDto>>>> GetDropDown()
        {
            var result = await _districtService.GetDropDown();
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpGet("GetByCity/{cityId:int}")]
        public async Task<ActionResult<Response<List<DistrictDto>>>> GetByCity(int cityId)
        {
            var result = await _districtService.GetByCity(cityId);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.DistrictClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<DistrictDto>>> CreateAsync(CreateDistrictDto entity)
        {
            var result = await _districtService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.DistrictClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<DistrictDto>>> UpdateAsync(int id, CreateDistrictDto entity)
        {
            if (id != entity.Id)
                return BadRequest("Id mismatch");

            var result = await _districtService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

    }
}
