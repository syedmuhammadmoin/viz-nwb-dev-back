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
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.PaymentClaims.Create, Permissions.PaymentClaims.View, Permissions.PaymentClaims.Delete, Permissions.PaymentClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<PaymentDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var payments = await _paymentService.GetAllAsync(filter, DocType.Payment );
            if (payments.IsSuccess)
                return Ok(payments); // Status Code : 200

            return BadRequest(payments); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.PaymentClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<PaymentDto>>> CreateAsync(CreatePaymentDto entity)
        {

            entity.PaymentType = PaymentType.Outflow;
            entity.PaymentFormType = DocType.Payment;
            var payment = await _paymentService.CreateAsync(entity);
            if (payment.IsSuccess)
                return Ok(payment); // Status Code : 200

            return BadRequest(payment); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.PaymentClaims.Create, Permissions.PaymentClaims.View, Permissions.PaymentClaims.Delete, Permissions.PaymentClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<PaymentDto>>> GetByIdAsync(int id)
        {
            var result = await _paymentService.GetByIdAsync(id, DocType.Payment);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.PaymentClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<PaymentDto>>> UpdateAsync(int id, CreatePaymentDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");


            entity.PaymentType = PaymentType.Outflow;
            entity.PaymentFormType = DocType.Payment;
            var result = await _paymentService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpPost("workflow")]
        public async Task<ActionResult<Response<bool>>> CheckWorkFlow([FromBody] ApprovalDto data)
        {
            var result = await _paymentService.CheckWorkFlow(data);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200
            return BadRequest(result);
        }

        [Authorize]
        [ClaimRequirement("Permission", new string[] { Permissions.BankReconClaims.Create, Permissions.BankReconClaims.View })]
        [HttpGet("bankStatus/{id:Guid}")]
        public ActionResult<Response<List<UnReconStmtDto>>> GetBankUnreconciledPayments(Guid id)
        {
            var result = _paymentService.GetBankUnreconciledPayments(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpPost("createProcess")]
        public async Task<ActionResult<bool>> CreatePayrollPaymentProcess([FromBody] CreatePayrollPaymentDto data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _paymentService.CreatePayrollPaymentProcess(data);
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

        [HttpPost("GetPayrollPayment")]
        public ActionResult<Response<List<PaymentDto>>> GetPaymentByDept(DeptFilter data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _paymentService.GetPaymentByDept(data);
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

        [HttpPost("GetPayrollTrans")]
        public ActionResult<Response<List<PayrollTransactionDto>>> GetPayrollTransactionByDept(DeptFilter data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _paymentService.GetPayrollTransactionByDept(data);
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

        [HttpPost("submitProcess")]
        public async Task<ActionResult<Response<bool>>> ProcessForEditPayrollPayment([FromBody] int[] id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _paymentService.ProcessForEditPayrollPayment(id);
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

        [HttpPost("GetforPayrollPaymentApproval")]
        public ActionResult<Response<bool>> GetPaymentForApproval(DeptFilter data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _paymentService.GetPaymentForApproval(data);
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
                    var result = await _paymentService.ProcessForApproval(data);
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
    }
}
