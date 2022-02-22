using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
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
        public async Task<ActionResult<List<PaymentDto>>> GetAllAsync([FromQuery] PaginationFilter filter)
        {
            var payment = await _paymentService.GetAllAsync(filter);
            return Ok(payment);
        }

        [HttpPost]
        public async Task<ActionResult<PaymentDto>> CreateAsync(CreatePaymentDto entity)
        {
            var payment = await _paymentService.CreateAsync(entity);
            return Ok(payment);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PaymentDto>> GetByIdAsync(int id)
        {
            var result = await _paymentService.GetByIdAsync(id);
            return Ok(result); // Status Code : 200
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<PaymentDto>> UpdateAsync(int id, CreatePaymentDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _paymentService.UpdateAsync(entity);
            return Ok(result); // Status Code : 200
        }

    }
}
