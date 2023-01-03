using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Helper;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Application.Services;
using Domain.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuotationComparativeController : ControllerBase
    {
        private readonly IQuotationComparativeService _quotationComparativeService;

        public QuotationComparativeController(IQuotationComparativeService quotationComparativeService)
        {
            _quotationComparativeService = quotationComparativeService;
        }
        [ClaimRequirement("Permission", new string[] { Permissions.QuotationComparativeClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<QuotationComparativeDto>>> CreateAsync(CreateQuotationComparativeDto entity)
        {
            var result = await _quotationComparativeService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }
        [ClaimRequirement("Permission", new string[] { Permissions.QuotationComparativeClaims.Create, Permissions.QuotationComparativeClaims.View, Permissions.QuotationComparativeClaims.Delete, Permissions.QuotationComparativeClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<QuotationComparativeDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var results = await _quotationComparativeService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }
        [ClaimRequirement("Permission", new string[] { Permissions.QuotationComparativeClaims.Create, Permissions.QuotationComparativeClaims.View, Permissions.QuotationComparativeClaims.Delete, Permissions.QuotationComparativeClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<QuotationComparativeDto>>> GetByIdAsync(int id)
        {
            var result = await _quotationComparativeService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }
        [ClaimRequirement("Permission", new string[] { Permissions.QuotationComparativeClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<QuotationComparativeDto>>> UpdateAsync(int id, CreateQuotationComparativeDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _quotationComparativeService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }
        [ClaimRequirement("Permission", new string[] { Permissions.QuotationComparativeClaims.Edit })]
        [HttpPut("AwardedVendor/{Id}")]
        public async Task<ActionResult<Response<AwardedVendorDto>>> AwardedVendorQuotation(int Id, AwardedVendorDto entity)
        {

            var result = await _quotationComparativeService.AwardedVendorQuotation
                (Id, entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

    }
}
