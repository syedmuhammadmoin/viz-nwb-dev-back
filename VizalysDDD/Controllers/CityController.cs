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
    public class CityController : ControllerBase
    {
        private readonly ICityService _cityService;

        public CityController(ICityService cityService)
        {
            _cityService = cityService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.CityClaims.Create, Permissions.CityClaims.View, Permissions.CityClaims.Delete, Permissions.CityClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<CityDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var results = await _cityService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.CityClaims.Create, Permissions.CityClaims.View, Permissions.CityClaims.Delete, Permissions.CityClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CityDto>> GetByIdAsync(int id)
        {
            var result = await _cityService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpGet("Dropdown")]
        public async Task<ActionResult<Response<List<CityDto>>>> GetDropDown()
        {
            var result = await _cityService.GetDropDown();
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpGet("GetByState/{stateId:int}")]
        public async Task<ActionResult<Response<List<CityDto>>>> GetByState(int stateId)
        {
            var result = await _cityService.GetByState(stateId);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.CityClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<CityDto>>> CreateAsync(CreateCityDto entity)
        {
            var result = await _cityService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.CityClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<CityDto>>> UpdateAsync(int id, CreateCityDto entity)
        {
            if (id != entity.Id)
                return BadRequest("Id mismatch");

            var result = await _cityService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

    }
}
