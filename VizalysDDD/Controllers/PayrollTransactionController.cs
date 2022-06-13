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
    public class PayrollTransactionController : ControllerBase
    {
        private readonly IPayrollTransactionService _payrollTransactionService;

        public PayrollTransactionController(IPayrollTransactionService invoiceService)
        {
            _payrollTransactionService = invoiceService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.PayrollTransactionClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<PayrollTransactionDto>>> CreateAsync(CreatePayrollTransactionDto entity)
        {
            var result = await _payrollTransactionService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.PayrollTransactionClaims.Create, Permissions.PayrollTransactionClaims.View, Permissions.PayrollTransactionClaims.Delete, Permissions.PayrollTransactionClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<PayrollTransactionDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var results = await _payrollTransactionService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.PayrollTransactionClaims.Create, Permissions.PayrollTransactionClaims.View, Permissions.PayrollTransactionClaims.Delete, Permissions.PayrollTransactionClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<PayrollTransactionDto>>> GetByIdAsync(int id)
        {
            var result = await _payrollTransactionService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.PayrollTransactionClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<PayrollTransactionDto>>> UpdateAsync(int id, CreatePayrollTransactionDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _payrollTransactionService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.PayrollTransactionClaims.View })]
        [HttpPost("workflow")]
        public async Task<ActionResult<Response<bool>>> CheckWorkFlow([FromBody] ApprovalDto data)
        {
            var result = await _payrollTransactionService.CheckWorkFlow(data);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200
            return BadRequest(result);
        }

        [HttpPost("submitProcess")]
        public async Task<ActionResult<Response<bool>>> ProcessForEdit([FromBody] int[] id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _payrollTransactionService.ProcessForEdit(id);
                    if (result.IsSuccess)
                        return Ok(result); // Status Code : 200
                    return BadRequest(result);
                }
                return BadRequest("Some properties are not valid"); // Status code : 400
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.Message);
            }
        }

        [HttpPost("approvalProcess")]
        public async Task<ActionResult<Response<bool>>> ProcessForApproval([FromBody] CreateApprovalProcessDto data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _payrollTransactionService.ProcessForApproval(data);
                    if (result.IsSuccess)
                        return Ok(result); // Status Code : 200
                    return BadRequest(result);
                }
                return BadRequest("Some properties are not valid"); // Status code : 400
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.Message);
            }
        }
        
        [ClaimRequirement("Permission", new string[] { Permissions.EmployeeClaims.View, Permissions.PayrollTransactionClaims.Create, Permissions.PayrollTransactionClaims.Edit })]
        [HttpPost("GetforSubmit")]
        public async Task<ActionResult<Response<List<PayrollTransactionDto>>>> GetEmployeeByDept(DeptFilter data)
        {
            try
            {
                var result = await _payrollTransactionService.GetEmployeesByDept(data);
                if (result.IsSuccess)
                    return Ok(result); // Status Code : 200

                return BadRequest(result); // Status code : 400
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.Message);
            }
        }

        [HttpPost("GetforApproval")]
        public ActionResult<Response<List<PayrollTransactionDto>>> GetPayrollTransactionByDept(DeptFilter data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result =  _payrollTransactionService.GetPayrollTransactionByDept(data);
                    if (result.IsSuccess)
                        return Ok(result); // Status Code : 200
                    return BadRequest(result);
                }
                return BadRequest("Some properties are not valid"); // Status code : 400
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.Message);
            }
        }
        [ClaimRequirement("Permission", new string[] { Permissions.PayrollTransactionClaims.Create, Permissions.PayrollTransactionClaims.View, Permissions.PayrollTransactionClaims.Delete, Permissions.PayrollTransactionClaims.Edit })]
        [HttpGet("Report")]
        public ActionResult<Response<List<PayrollTransactionDto>>> GetPayrollReport([FromQuery] PayrollFilter filter)
        {
            try
            {
                var result = _payrollTransactionService.GetPayrollReport(filter);
                if (result.IsSuccess)
                    return Ok(result); // Status Code : 200

                return BadRequest(result); // Status code : 400
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.Message);
            }
        }

    }
}
