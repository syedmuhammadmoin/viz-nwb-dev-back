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
        public async Task<ActionResult<PaginationResponse<List<PaymentDto>>>> GetAllAsync([FromQuery] PaginationFilter filter)
        {
            var payments = await _paymentService.GetAllAsync(filter, PaymentType.Outflow );
            if (payments.IsSuccess)
                return Ok(payments); // Status Code : 200

            return BadRequest(payments); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.PaymentClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<PaymentDto>>> CreateAsync(CreatePaymentDto entity)
        {
            if (entity.PaymentType != PaymentType.Outflow)
            {
                return new Response<PaymentDto>("Invalid API");
            }

            var payment = await _paymentService.CreateAsync(entity);
            if (payment.IsSuccess)
                return Ok(payment); // Status Code : 200

            return BadRequest(payment); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.PaymentClaims.View, Permissions.PaymentClaims.Delete, Permissions.PaymentClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<PaymentDto>>> GetByIdAsync(int id)
        {
            var result = await _paymentService.GetByIdAsync(id, PaymentType.Outflow);
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

            if (entity.PaymentType != PaymentType.Outflow)
            {
                return new Response<PaymentDto>("Invalid API");
            }
            var result = await _paymentService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.PaymentClaims.View })]
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
