using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Helper;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
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
        private readonly IConfiguration _configuration;

        public DepartmentController(IDepartmentService departmentService, IConfiguration configuration)
        {
            _departmentService = departmentService;
            _configuration = configuration;
        }

        [EnableCors("PayrollModule")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<DepartmentDto>>>> GetAllAsync([FromHeader(Name = "key")] string key, [FromQuery] TransactionFormFilter filter)
        {
            if (key != _configuration["ApiKey:Key"])
            {
                return BadRequest("Invalid Key");
            }
            var departments = await _departmentService.GetAllAsync(filter);
            if (departments.IsSuccess)
                return Ok(departments); // Status Code : 200

            return BadRequest(departments); // Status code : 400
        }

        [EnableCors("PayrollModule")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<Response<DepartmentDto>>> CreateAsync([FromHeader(Name = "key")] string key, CreateDepartmentDto[] entity)
        {
            if (key != _configuration["ApiKey:Key"])
            {
                return BadRequest("Invalid Key");
            }
            var department = await _departmentService.CreateAsync(entity);
            if (department.IsSuccess)
                return Ok(department); // Status Code : 200

            return BadRequest(department); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.DepartmentClaims.View, Permissions.DepartmentClaims.Delete, Permissions.DepartmentClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<DepartmentDto>>> GetByIdAsync(int id)
        {
            var result = await _departmentService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpGet("Dropdown")]
        public async Task<ActionResult<Response<List<DepartmentDto>>>> GetDepartmentDropDown()
        {
            return Ok(await _departmentService.GetDepartmentDropDown()); // Status Code : 200
        }

        [HttpGet("GetDepartmentByCampus/{id:int}")]
        public async Task<ActionResult<Response<List<WarehouseDto>>>> GetDepartmentByCampusDropDown(int id)
        {
            return Ok(await _departmentService.GetDepartmentByCampusDropDown(id)); // Status Code : 200
        }
    }
}
