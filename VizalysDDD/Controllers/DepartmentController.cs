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

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [EnableCors("PayrollModule")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<DepartmentDto>>>> GetAllAsync([FromHeader(Name = "key")] string key, [FromQuery] TransactionFormFilter filter)
        {
            if (key != "b4!V47w^e3QhItW_XY:jHgWQp%$&93nMS|h)Bj~R0&Q#J1m%lI^;b4C,&]Gf2(H_fu]5&X@1Oy~")
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
        public async Task<ActionResult<Response<DepartmentDto>>> CreateAsync([FromHeader(Name = "key")] string key, DepartmentDto[] entity)
        {
            if (key != "b4!V47w^e3QhItW_XY:jHgWQp%$&93nMS|h)Bj~R0&Q#J1m%lI^;b4C,&]Gf2(H_fu]5&X@1Oy~")
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
    }
}
