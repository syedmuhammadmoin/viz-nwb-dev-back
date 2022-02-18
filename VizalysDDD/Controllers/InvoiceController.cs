using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpPost]
        public async Task<ActionResult<InvoiceDto>> CreateAsync(CreateInvoiceDto entity)
        {
            var invoice = await _invoiceService.CreateAsync(entity);
            return Ok(invoice);
        }

        [HttpGet]
        public async Task<ActionResult<List<InvoiceDto>>> GetAllAsync([FromQuery] PaginationFilter filter)
        {
            var invs = await _invoiceService.GetAllAsync(filter);
            return Ok(invs);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<InvoiceDto>> GetByIdAsync(int id)
        {
            var result = await _invoiceService.GetByIdAsync(id);
            return Ok(result); // Status Code : 200
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<InvoiceDto>> UpdateAsync(int id, CreateInvoiceDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _invoiceService.UpdateAsync(entity);
            return Ok(result); // Status Code : 200
        }
    }
}
