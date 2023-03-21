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
    public class ProgramController : ControllerBase
    {
        private readonly IProgramService _programService;

        public ProgramController(IProgramService programService)
        {
            _programService = programService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.ProgramClaims.Create, Permissions.ProgramClaims.View, Permissions.ProgramClaims.Delete, Permissions.ProgramClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<ProgramDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var results = await _programService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.ProgramClaims.Create, Permissions.ProgramClaims.View, Permissions.ProgramClaims.Delete, Permissions.ProgramClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProgramDto>> GetByIdAsync(int id)
        {
            var result = await _programService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpGet("Dropdown")]
        public async Task<ActionResult<Response<List<ProgramDto>>>> GetDropDown()
        {
            var result = await _programService.GetDropDown();
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.ProgramClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<ProgramDto>>> CreateAsync(CreateProgramDto entity)
        {
            var result = await _programService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.ProgramClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<ProgramDto>>> UpdateAsync(int id, CreateProgramDto entity)
        {
            if (id != entity.Id)
                return BadRequest("Id mismatch");

            var result = await _programService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

    }
}
