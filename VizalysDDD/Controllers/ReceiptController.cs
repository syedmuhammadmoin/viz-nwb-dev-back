﻿using Application.Contracts.DTOs;
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

    public class ReceiptController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public ReceiptController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.ReceiptClaims.Create, Permissions.ReceiptClaims.View, Permissions.ReceiptClaims.Delete, Permissions.ReceiptClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<PaymentDto>>>> GetAllAsync([FromQuery] PaginationFilter filter)
        {
            var payments = await _paymentService.GetAllAsync(filter, DocType.Receipt);
            if (payments.IsSuccess)
                return Ok(payments); // Status Code : 200

            return BadRequest(payments); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.ReceiptClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<PaymentDto>>> CreateAsync(CreatePaymentDto entity)
        {
            entity.PaymentType = PaymentType.Inflow;
            entity.PaymentFormType = DocType.Receipt;
            var payment = await _paymentService.CreateAsync(entity);
            if (payment.IsSuccess)
                return Ok(payment); // Status Code : 200

            return BadRequest(payment); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.ReceiptClaims.View, Permissions.ReceiptClaims.Delete, Permissions.ReceiptClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<PaymentDto>>> GetByIdAsync(int id)
        {
            var result = await _paymentService.GetByIdAsync(id, DocType.Receipt);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.ReceiptClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<PaymentDto>>> UpdateAsync(int id, CreatePaymentDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            entity.PaymentType = PaymentType.Inflow;
            entity.PaymentFormType = DocType.Receipt;

            var result = await _paymentService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.ReceiptClaims.View })]
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
