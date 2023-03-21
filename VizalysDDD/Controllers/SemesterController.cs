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
    public class SemesterController : ControllerBase
    {
        private readonly ISemesterService _semesterService;

        public SemesterController(ISemesterService semesterService)
        {
            _semesterService = semesterService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.SemesterClaims.Create, Permissions.SemesterClaims.View, Permissions.SemesterClaims.Delete, Permissions.SemesterClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<SemesterDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var results = await _semesterService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.SemesterClaims.Create, Permissions.SemesterClaims.View, Permissions.SemesterClaims.Delete, Permissions.SemesterClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<SemesterDto>> GetByIdAsync(int id)
        {
            var result = await _semesterService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpGet("Dropdown")]
        public async Task<ActionResult<Response<List<SemesterDto>>>> GetDropDown()
        {
            var result = await _semesterService.GetDropDown();
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.SemesterClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<SemesterDto>>> CreateAsync(SemesterDto entity)
        {
            var result = await _semesterService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.SemesterClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<SemesterDto>>> UpdateAsync(int id, SemesterDto entity)
        {
            if (id != entity.Id)
                return BadRequest("Id mismatch");

            var result = await _semesterService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

    }
}
