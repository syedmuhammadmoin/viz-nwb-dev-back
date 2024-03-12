using Application.Contracts.DTOs;
using Application.Contracts.DTOs.PayrollTransaction;
using Application.Contracts.Filters;
using Application.Contracts.Helper;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace Vizalys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PayrollTransactionController : ControllerBase
    {
        private readonly IPayrollTransactionService _payrollTransactionService;
        private readonly IFileuploadServices _fileUploadService;
        private readonly IConfiguration _configuration;

        public PayrollTransactionController(IPayrollTransactionService invoiceService, IFileuploadServices fileUploadService, IConfiguration configuration)
        {
            _payrollTransactionService = invoiceService;
            _fileUploadService = fileUploadService;
            _configuration = configuration;
        }        
        [EnableCors("PayrollModule")]
        [AllowAnonymous]
        //[ClaimRequirement("Permission", new string[] { Permissions.PayrollTransactionClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<PayrollTransactionDto>>> CreateAsync([FromHeader(Name = "key")] string key, CreatePayrollTransactionDto[] entity)
        {
            if (key != _configuration["ApiKey:Key"])
            {
                return BadRequest("Invalid Key");
            }

            var result = await _payrollTransactionService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }
        [ClaimRequirement("Permission", new string[] { Permissions.PayrollTransactionClaims.Edit })]
        [HttpPost("updateTransaction")]

        public async Task<ActionResult<Response<PayrollTransactionDto>>> UpdateTransaction([FromHeader(Name = "key")] string key, UpdateEmployeeTransactionDto entity)
        {                   
            if (key != _configuration["ApiKey:Key"])
            {
                return BadRequest("Invalid Key");
            }

            var result = await _payrollTransactionService.UpdatePayrollTransaction(entity.Id,entity , 1);
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
        public async Task<ActionResult<Response<PayrollTransactionDto>>> UpdateAsync(int id, UpdatePayrollTransactionDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _payrollTransactionService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

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
        public async Task<ActionResult<Response<Object>>> GetEmployeeByDept(DeptFilter data)
        {
            try
            {
                var result = await _payrollTransactionService.GetPayrollReport(data);
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
                    var result = _payrollTransactionService.GetPayrollTransactionByDept(data);
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

        [HttpGet("DetailReport")]
        public ActionResult<Response<Object>>GetPayrollReport([FromQuery] PayrollDetailFilter filter)
        {
            try
            {
                var result = _payrollTransactionService.GetPayrollDetailReport(filter);
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
        [HttpGet("ExportPayrollDetailedReport")]
        public async Task<ActionResult> ExportPayrollDetailedReport([FromQuery] PayrollDetailFilter filter)
        {
            try
            {
                var stream = await _payrollTransactionService.ExportPayrollDetailedReport(filter);
                string fromDate = filter.FromDate.Value.Date.ToString("dd MMMM yyyy");
                string toDate = filter.FromDate.Value.Date.ToString("dd MMMM yyyy");
                string excelName = $"PayrollDetailedReport-{fromDate}-till-{toDate}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.Message);
            }
        }

        
        [HttpGet("CampusReport")]
        public ActionResult<Response<List<PayrollExecutiveReportDto>>> GetPayrollCampusReport([FromQuery] PayrollCampusReportFilter filter)
        {
            var result = _payrollTransactionService.GetPayrollCampusGroupReport(filter);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }
        [HttpPost("PayrollExecutiveReport")]
        public ActionResult<Response<List<PayrollExecutiveReportDto>>> GetPayrollExecutiveReport(PayrollExecutiveReportFilter filter)
        {
            var result = _payrollTransactionService.GetPayrollExecutiveReport(filter);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpPost("DocUpload/{id:int}")]
        public async Task<ActionResult<Response<int>>> UploadFile(IFormFile file, int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _fileUploadService.UploadFile(file, id, DocType.PayrollTransaction);
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

        [HttpGet("BankAdviceReport")]
        public  ActionResult<Response<List<PayrollTransactionDto>>> GetBankAdviceReportReport([FromQuery] BankAdviceReportFilter filter)
        {
            var result = _payrollTransactionService.GetBankAdviceReportReport(filter);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }
    }
}
