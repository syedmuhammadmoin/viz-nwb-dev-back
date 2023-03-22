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
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.SubjectClaims.Create, Permissions.SubjectClaims.View, Permissions.SubjectClaims.Delete, Permissions.SubjectClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<SubjectDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var results = await _subjectService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.SubjectClaims.Create, Permissions.SubjectClaims.View, Permissions.SubjectClaims.Delete, Permissions.SubjectClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<SubjectDto>> GetByIdAsync(int id)
        {
            var result = await _subjectService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpGet("Dropdown")]
        public async Task<ActionResult<Response<List<SubjectDto>>>> GetDropDown()
        {
            var result = await _subjectService.GetDropDown();
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.SubjectClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<SubjectDto>>> CreateAsync(SubjectDto entity)
        {
            var result = await _subjectService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.SubjectClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<SubjectDto>>> UpdateAsync(int id, SubjectDto entity)
        {
            if (id != entity.Id)
                return BadRequest("Id mismatch");

            var result = await _subjectService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

    }
}
