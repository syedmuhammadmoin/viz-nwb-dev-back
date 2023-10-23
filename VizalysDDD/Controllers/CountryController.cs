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
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.CountryClaims.Create, Permissions.CountryClaims.View, Permissions.CountryClaims.Delete, Permissions.CountryClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<CountryDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var results = await _countryService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.CountryClaims.Create, Permissions.CountryClaims.View, Permissions.CountryClaims.Delete, Permissions.CountryClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CountryDto>> GetByIdAsync(int id)
        {
            var result = await _countryService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpGet("Dropdown")]
        public async Task<ActionResult<Response<List<CountryDto>>>> GetDropDown()
        {
            var result = await _countryService.GetDropDown();
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.CountryClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<CountryDto>>> CreateAsync(CountryDto entity)
        {
            var result = await _countryService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.CountryClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<CountryDto>>> UpdateAsync(int id, CountryDto entity)
        {
            if (id != entity.Id)
                return BadRequest("Id mismatch");

            var result = await _countryService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

    }
}
