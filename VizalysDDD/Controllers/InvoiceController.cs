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
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.InvoiceClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<InvoiceDto>>> CreateAsync(CreateInvoiceDto entity)
        {
            var result = await _invoiceService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.InvoiceClaims.Create, Permissions.InvoiceClaims.View, Permissions.InvoiceClaims.Delete, Permissions.InvoiceClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<InvoiceDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var results = await _invoiceService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.InvoiceClaims.Create, Permissions.InvoiceClaims.View, Permissions.InvoiceClaims.Delete, Permissions.InvoiceClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<InvoiceDto>>> GetByIdAsync(int id)
        {
            var result = await _invoiceService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.InvoiceClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<InvoiceDto>>> UpdateAsync(int id, CreateInvoiceDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _invoiceService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.InvoiceClaims.View })]
        [HttpPost("workflow")]
        public async Task<ActionResult<Response<bool>>> CheckWorkFlow([FromBody] ApprovalDto data)
        {
            var result = await _invoiceService.CheckWorkFlow(data);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200
            return BadRequest(result);
        }
    }
}
