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
    public class ProgramChallanTemplateController : ControllerBase
    {
        private readonly IProgramChallanTemplateService _programChallanTemplateService;

        public ProgramChallanTemplateController(IProgramChallanTemplateService programChallanTemplateService)
        {
            _programChallanTemplateService = programChallanTemplateService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.ProgramChallanTemplateClaims.Create, Permissions.ProgramChallanTemplateClaims.View, Permissions.ProgramChallanTemplateClaims.Delete, Permissions.ProgramChallanTemplateClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<ProgramChallanTemplateDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var results = await _programChallanTemplateService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.ProgramChallanTemplateClaims.Create, Permissions.ProgramChallanTemplateClaims.View, Permissions.ProgramChallanTemplateClaims.Delete, Permissions.ProgramChallanTemplateClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProgramChallanTemplateDto>> GetByIdAsync(int id)
        {
            var result = await _programChallanTemplateService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.ProgramChallanTemplateClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<ProgramChallanTemplateDto>>> CreateAsync(CreateProgramChallanTemplateDto entity)
        {
            var result = await _programChallanTemplateService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.ProgramChallanTemplateClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<ProgramChallanTemplateDto>>> UpdateAsync(int id, CreateProgramChallanTemplateDto entity)
        {
            if (id != entity.Id)
                return BadRequest("Id mismatch");

            var result = await _programChallanTemplateService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

    }
}
