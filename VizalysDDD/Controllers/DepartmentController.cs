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
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.DepartmentsClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<DeptDto>>> CreateAsync(CreateDeptDto entity)
        {
            var result = await _departmentService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.DepartmentsClaims.Create, Permissions.DepartmentsClaims.View, Permissions.DepartmentsClaims.Delete, Permissions.DepartmentsClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<DeptDto>>>> GetAllAsync([FromQuery] PaginationFilter filter)
        {
            var results = await _departmentService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.DepartmentsClaims.View, Permissions.DepartmentsClaims.Delete, Permissions.DepartmentsClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<DeptDto>>> GetByIdAsync(int id)
        {
            var result = await _departmentService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.DepartmentsClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<DeptDto>>> UpdateAsync(int id, CreateDeptDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _departmentService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpGet("Dropdown")]
        public async Task<ActionResult<Response<List<DeptDto>>>> GetDepartmentDropDown()
        {
            return Ok(await _departmentService.GetDepartmentDropDown()); // Status Code : 200
        }
    }
}
