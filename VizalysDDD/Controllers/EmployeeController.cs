using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Helper;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [EnableCors("PayrollModule")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<EmployeeDto>>>> GetAllAsync([FromHeader(Name = "key")] string key, [FromQuery] TransactionFormFilter filter)
        {
            if (key != "b4!V47w^e3QhItW_XY:jHgWQp%$&93nMS|h)Bj~R0&Q#J1m%lI^;b4C,&]Gf2(H_fu]5&X@1Oy~")
            {
                return BadRequest("Invalid Key");
            }
            var employees = await _employeeService.GetAllAsync(filter);
            if (employees.IsSuccess)
                return Ok(employees); // Status Code : 200

            return BadRequest(employees); // Status code : 400
        }

        [EnableCors("PayrollModule")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<Response<EmployeeDto>>> CreateAsync([FromHeader(Name = "key")] string key, CreateEmployeeDto[] entity)
        {
            if (key != "b4!V47w^e3QhItW_XY:jHgWQp%$&93nMS|h)Bj~R0&Q#J1m%lI^;b4C,&]Gf2(H_fu]5&X@1Oy~")
            {
                return BadRequest("Invalid Key");
            }
            var employee = await _employeeService.CreateAsync(entity);
            if (employee.IsSuccess)
                return Ok(employee); // Status Code : 200

            return BadRequest(employee); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.PayrollTransactionClaims.Create, Permissions.PayrollTransactionClaims.Edit, Permissions.EmployeeClaims.View, Permissions.EmployeeClaims.Delete, Permissions.EmployeeClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<EmployeeDto>>> GetByIdAsync(int id)
        {
            var result = await _employeeService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpGet("Dropdown")]
        public async Task<ActionResult<Response<List<EmployeeDto>>>> GetEmployeeDropDown()
        {
            return Ok(await _employeeService.GetEmployeeDropDown()); // Status Code : 200
        }

        [HttpGet("EmployeePaymentDropDown")]
        public async Task<ActionResult<Response<List<EmployeeDto>>>> GetEmployeeDropDownPayment()
        {
            return Ok(await _employeeService.GetEmployeeDropDownPayment()); // Status Code : 200
        }

    }
}
