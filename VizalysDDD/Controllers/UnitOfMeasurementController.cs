using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Helper;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class UnitOfMeasurementController : ControllerBase
    {
        private readonly IUnitOfMeasurementService _unitOfMeasurementService;

        public UnitOfMeasurementController(IUnitOfMeasurementService unitOfMeasurementService)
        {
            _unitOfMeasurementService = unitOfMeasurementService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.UnitOfMeasurementClaims.Create, Permissions.UnitOfMeasurementClaims.View, Permissions.UnitOfMeasurementClaims.Delete, Permissions.UnitOfMeasurementClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<UnitOfMeasurementDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var unitOfMeasurements = await _unitOfMeasurementService.GetAllAsync(filter);
            if (unitOfMeasurements.IsSuccess)
                return Ok(unitOfMeasurements); // Status Code : 200

            return BadRequest(unitOfMeasurements); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.UnitOfMeasurementClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<UnitOfMeasurementDto>>> CreateAsync(CreateUnitOfMeasurementDto entity)
        {
            var unitOfMeasurement = await _unitOfMeasurementService.CreateAsync(entity);
            if (unitOfMeasurement.IsSuccess)
                return Ok(unitOfMeasurement); // Status Code : 200

            return BadRequest(unitOfMeasurement); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.UnitOfMeasurementClaims.Create, Permissions.UnitOfMeasurementClaims.View, Permissions.UnitOfMeasurementClaims.Delete, Permissions.UnitOfMeasurementClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<UnitOfMeasurementDto>>> GetByIdAsync(int id)
        {
            var result = await _unitOfMeasurementService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.UnitOfMeasurementClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<UnitOfMeasurementDto>>> UpdateAsync(int id, CreateUnitOfMeasurementDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _unitOfMeasurementService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpGet("Dropdown")]
        public async Task<ActionResult<Response<List<UnitOfMeasurementDto>>>> GetUnitOfMeasurementDropDown()
        {
            return Ok(await _unitOfMeasurementService.GetUnitOfMeasurementDropDown()); // Status Code : 200
        }
    }
}
