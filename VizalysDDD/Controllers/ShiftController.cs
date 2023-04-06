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
    public class ShiftController : ControllerBase
    {
        private readonly IShiftService _shiftService;

        public ShiftController(IShiftService shiftService)
        {
            _shiftService = shiftService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.ShiftClaims.Create, Permissions.ShiftClaims.View, Permissions.ShiftClaims.Delete, Permissions.ShiftClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<ShiftDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var results = await _shiftService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.ShiftClaims.Create, Permissions.ShiftClaims.View, Permissions.ShiftClaims.Delete, Permissions.ShiftClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ShiftDto>> GetByIdAsync(int id)
        {
            var result = await _shiftService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpGet("Dropdown")]
        public async Task<ActionResult<Response<List<ShiftDto>>>> GetDropDown()
        {
            var result = await _shiftService.GetDropDown();
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.ShiftClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<ShiftDto>>> CreateAsync(ShiftDto entity)
        {
            var result = await _shiftService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.ShiftClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<ShiftDto>>> UpdateAsync(int id, ShiftDto entity)
        {
            if (id != entity.Id)
                return BadRequest("Id mismatch");

            var result = await _shiftService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

    }
}
