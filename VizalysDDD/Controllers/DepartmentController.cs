using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<ActionResult<List<DeptDto>>> GetAllAsync([FromQuery] PaginationFilter filter)
        {
            var department = await _departmentService.GetAllAsync(filter);
            return Ok(department);
        }
        [HttpPost]
        public async Task<ActionResult<DeptDto>> CreateAsync(CreateDeptDto entity)
        {
            var department = await _departmentService.CreateAsync(entity);
            return Ok(department);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<DeptDto>> GetByIdAsync(int id)
        {
            var result = await _departmentService.GetByIdAsync(id);
            return Ok(result); // Status Code : 200
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<DeptDto>> UpdateAsync(int id, CreateDeptDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _departmentService.UpdateAsync(entity);
            return Ok(result); // Status Code : 200
        }
    }
}
