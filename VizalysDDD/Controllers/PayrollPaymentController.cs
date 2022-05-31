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

    public class PayrollPaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PayrollPaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        [ClaimRequirement("Permission", new string[] { Permissions.PayrollPaymentClaims.Create, Permissions.PayrollPaymentClaims.View, Permissions.PayrollPaymentClaims.Delete, Permissions.PayrollPaymentClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<PaymentDto>>>> GetAllAsync([FromQuery] PaginationFilter filter)
        {
            var payments = await _paymentService.GetAllAsync(filter, PaymentType.Outflow, DocType.PayrollPayment);
            if (payments.IsSuccess)
                return Ok(payments); // Status Code : 200

            return BadRequest(payments); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.PayrollPaymentClaims.Create, Permissions.PayrollPaymentClaims.View, Permissions.PayrollPaymentClaims.Delete, Permissions.PayrollPaymentClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<PaginationResponse<List<PaymentDto>>>> GetByIdAsync (int id)
        {
            var payments = await _paymentService.GetByIdAsync(id, PaymentType.Outflow, DocType.PayrollPayment);
            if (payments.IsSuccess)
                return Ok(payments); // Status Code : 200

            return BadRequest(payments); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.PayrollPaymentClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<PaymentDto>>> CreateAsync(CreatePaymentDto entity)
        {
            if (entity.PaymentType != PaymentType.Outflow && entity.PaymentFormType != DocType.PayrollPayment)
            {
                return new Response<PaymentDto>("Invalid API");
            }

            var payment = await _paymentService.CreateAsync(entity);
            if (payment.IsSuccess)
                return Ok(payment); // Status Code : 200

            return BadRequest(payment); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.PayrollPaymentClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<PaymentDto>>> UpdateAsync(int id, CreatePaymentDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            if (entity.PaymentType != PaymentType.Outflow && entity.PaymentFormType != DocType.PayrollPayment)
            {
                return new Response<PaymentDto>("Invalid API");
            }
            var result = await _paymentService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }


        [ClaimRequirement("Permission", new string[] { Permissions.PayrollPaymentClaims.View })]
        [HttpPost("workflow")]
        public async Task<ActionResult<Response<bool>>> CheckWorkFlow([FromBody] ApprovalDto data)
        {
            var result = await _paymentService.CheckWorkFlow(data);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200
            return BadRequest(result);
        }
    }
}
