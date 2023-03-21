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
    public class DegreeController : ControllerBase
    {
        private readonly IDegreeService _degreeService;

        public DegreeController(IDegreeService degreeService)
        {
            _degreeService = degreeService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.DegreeClaims.Create, Permissions.DegreeClaims.View, Permissions.DegreeClaims.Delete, Permissions.DegreeClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<DegreeDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var results = await _degreeService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.DegreeClaims.Create, Permissions.DegreeClaims.View, Permissions.DegreeClaims.Delete, Permissions.DegreeClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<DegreeDto>> GetByIdAsync(int id)
        {
            var result = await _degreeService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpGet("Dropdown")]
        public async Task<ActionResult<Response<List<DegreeDto>>>> GetDropDown()
        {
            var result = await _degreeService.GetDropDown();
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.DegreeClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<DegreeDto>>> CreateAsync(DegreeDto entity)
        {
            var result = await _degreeService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.DegreeClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<DegreeDto>>> UpdateAsync(int id, DegreeDto entity)
        {
            if (id != entity.Id)
                return BadRequest("Id mismatch");

            var result = await _degreeService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

    }
}
