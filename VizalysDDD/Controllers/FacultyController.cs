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
    public class FacultyController : ControllerBase
    {
        private readonly IFacultyService _facultyService;

        public FacultyController(IFacultyService facultyService)
        {
            _facultyService = facultyService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.FacultyClaims.Create, Permissions.FacultyClaims.View, Permissions.FacultyClaims.Delete, Permissions.FacultyClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<FacultyDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var results = await _facultyService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.FacultyClaims.Create, Permissions.FacultyClaims.View, Permissions.FacultyClaims.Delete, Permissions.FacultyClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<FacultyDto>> GetByIdAsync(int id)
        {
            var result = await _facultyService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpGet("Dropdown")]
        public async Task<ActionResult<Response<List<FacultyDto>>>> GetDropDown()
        {
            var result = await _facultyService.GetDropDown();
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.FacultyClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<FacultyDto>>> CreateAsync(CreateFacultyDto entity)
        {
            var result = await _facultyService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }
        
        [ClaimRequirement("Permission", new string[] { Permissions.FacultyClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<FacultyDto>>> UpdateAsync(int id, CreateFacultyDto entity)
        {
            if (id != entity.Id)
                return BadRequest("Id mismatch");

            var result = await _facultyService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }
    
    }
}
