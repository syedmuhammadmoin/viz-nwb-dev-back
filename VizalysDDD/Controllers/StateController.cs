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
    public class StateController : ControllerBase
    {
        private readonly IStateService _stateService;

        public StateController(IStateService stateService)
        {
            _stateService = stateService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.StateClaims.Create, Permissions.StateClaims.View, Permissions.StateClaims.Delete, Permissions.StateClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<StateDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var results = await _stateService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.StateClaims.Create, Permissions.StateClaims.View, Permissions.StateClaims.Delete, Permissions.StateClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<StateDto>> GetByIdAsync(int id)
        {
            var result = await _stateService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpGet("Dropdown")]
        public async Task<ActionResult<Response<List<StateDto>>>> GetDropDown()
        {
            var result = await _stateService.GetDropDown();
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpGet("GetByCountry/{countryId:int}")]
        public async Task<ActionResult<Response<List<StateDto>>>> GetByCountry(int countryId)
        {
            var result = await _stateService.GetByCountry(countryId);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.StateClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<StateDto>>> CreateAsync(CreateStateDto entity)
        {
            var result = await _stateService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.StateClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<StateDto>>> UpdateAsync(int id, CreateStateDto entity)
        {
            if (id != entity.Id)
                return BadRequest("Id mismatch");

            var result = await _stateService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

    }
}
