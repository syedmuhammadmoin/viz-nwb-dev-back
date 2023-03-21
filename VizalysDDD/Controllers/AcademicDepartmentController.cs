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
    public class AcademicDepartmentController : ControllerBase
    {
        private readonly IAcademicDepartmentService _academicDepartmentService;

        public AcademicDepartmentController(IAcademicDepartmentService academicDepartmentService)
        {
            _academicDepartmentService = academicDepartmentService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.AcademicDepartmentClaims.Create, Permissions.AcademicDepartmentClaims.View, Permissions.AcademicDepartmentClaims.Delete, Permissions.AcademicDepartmentClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<AcademicDepartmentDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var results = await _academicDepartmentService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.AcademicDepartmentClaims.Create, Permissions.AcademicDepartmentClaims.View, Permissions.AcademicDepartmentClaims.Delete, Permissions.AcademicDepartmentClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<AcademicDepartmentDto>> GetByIdAsync(int id)
        {
            var result = await _academicDepartmentService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpGet("Dropdown")]
        public async Task<ActionResult<Response<List<AcademicDepartmentDto>>>> GetDropDown()
        {
            var result = await _academicDepartmentService.GetDropDown();
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.AcademicDepartmentClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<AcademicDepartmentDto>>> CreateAsync(CreateAcademicDepartmentDto entity)
        {
            var result = await _academicDepartmentService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.AcademicDepartmentClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<AcademicDepartmentDto>>> UpdateAsync(int id, CreateAcademicDepartmentDto entity)
        {
            if (id != entity.Id)
                return BadRequest("Id mismatch");

            var result = await _academicDepartmentService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

    }
}
