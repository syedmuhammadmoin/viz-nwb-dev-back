using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Helper;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Domain.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.CourseClaims.Create, Permissions.CourseClaims.View, Permissions.CourseClaims.Delete, Permissions.CourseClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<CourseDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var results = await _courseService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.CourseClaims.Create, Permissions.CourseClaims.View, Permissions.CourseClaims.Delete, Permissions.CourseClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CourseDto>> GetByIdAsync(int id)
        {
            var result = await _courseService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpGet("Dropdown")]
        public async Task<ActionResult<Response<List<CourseDto>>>> GetDropDown()
        {
            var result = await _courseService.GetDropDown();
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.CourseClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<CourseDto>>> CreateAsync(CourseDto entity)
        {
            var result = await _courseService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.CourseClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<CourseDto>>> UpdateAsync(int id, CourseDto entity)
        {
            if (id != entity.Id)
                return BadRequest("Id mismatch");

            var result = await _courseService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

    }
}
