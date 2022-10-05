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
        private readonly IConfiguration _configuration;

        public EmployeeController(IEmployeeService employeeService, IConfiguration configuration)
        {
            _employeeService = employeeService;
            _configuration = configuration;
        }

        [EnableCors("PayrollModule")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<EmployeeDto>>>> GetAllAsync([FromHeader(Name = "key")] string key, [FromQuery] TransactionFormFilter filter)
        {
            if (key != _configuration["ApiKey:Key"])
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
            if (key != _configuration["ApiKey:Key"])
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

        [ClaimRequirement("Permission", new string[] { Permissions.EmployeeClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<CampusDto>>> UpdateAsync(int id, UpdateEmployeeDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _employeeService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }


    }
}
