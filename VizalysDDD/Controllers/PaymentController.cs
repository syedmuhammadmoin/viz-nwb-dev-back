﻿using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<PaymentDto>>>> GetAllAsync([FromQuery] PaginationFilter filter)
        {
            var payments = await _paymentService.GetAllAsync(filter);
            if (payments.IsSuccess)
                return Ok(payments); // Status Code : 200

            return BadRequest(payments); // Status code : 400
        }

        [HttpPost]
        public async Task<ActionResult<Response<PaymentDto>>> CreateAsync(CreatePaymentDto entity)
        {
            var payment = await _paymentService.CreateAsync(entity);
            if (payment.IsSuccess)
                return Ok(payment); // Status Code : 200

            return BadRequest(payment); // Status code : 400
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<PaymentDto>>> GetByIdAsync(int id)
        {
            var result = await _paymentService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<PaymentDto>>> UpdateAsync(int id, CreatePaymentDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _paymentService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

    }
}