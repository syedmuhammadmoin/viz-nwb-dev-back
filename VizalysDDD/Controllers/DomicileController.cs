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
    public class DomicileController : ControllerBase
    {
        private readonly IDomicileService _domicileService;

        public DomicileController(IDomicileService domicileService)
        {
            _domicileService = domicileService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.DomicileClaims.Create, Permissions.DomicileClaims.View, Permissions.DomicileClaims.Delete, Permissions.DomicileClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<DomicileDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var results = await _domicileService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.DomicileClaims.Create, Permissions.DomicileClaims.View, Permissions.DomicileClaims.Delete, Permissions.DomicileClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<DomicileDto>> GetByIdAsync(int id)
        {
            var result = await _domicileService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpGet("Dropdown")]
        public async Task<ActionResult<Response<List<DomicileDto>>>> GetDropDown()
        {
            var result = await _domicileService.GetDropDown();
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpGet("GetByDistrict/{districtId:int}")]
        public async Task<ActionResult<Response<List<DomicileDto>>>> GetByDistrict(int districtId)
        {
            var result = await _domicileService.GetByDistrict(districtId);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.DomicileClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<DomicileDto>>> CreateAsync(CreateDomicileDto entity)
        {
            var result = await _domicileService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.DomicileClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<DomicileDto>>> UpdateAsync(int id, CreateDomicileDto entity)
        {
            if (id != entity.Id)
                return BadRequest("Id mismatch");

            var result = await _domicileService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

    }
}
