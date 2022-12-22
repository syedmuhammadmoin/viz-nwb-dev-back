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
    public class QuotationController : ControllerBase
    {
        private readonly IQuotationService _quotationService;

        public QuotationController(IQuotationService quotationService)
        {
            _quotationService = quotationService;
        }
        [ClaimRequirement("Permission", new string[] { Permissions.QuotationClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<QuotationDto>>> CreateAsync(CreateQuotationDto entity)
        {
            var result = await _quotationService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }
        [ClaimRequirement("Permission", new string[] { Permissions.QuotationClaims.Create, Permissions.QuotationClaims.View, Permissions.QuotationClaims.Delete, Permissions.QuotationClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<QuotationDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var results = await _quotationService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }
        [ClaimRequirement("Permission", new string[] { Permissions.QuotationClaims.Create, Permissions.QuotationClaims.View, Permissions.QuotationClaims.Delete, Permissions.QuotationClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<QuotationDto>>> GetByIdAsync(int id)
        {
            var result = await _quotationService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }
        [ClaimRequirement("Permission", new string[] { Permissions.QuotationClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<QuotationDto>>> UpdateAsync(int id, CreateQuotationDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _quotationService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpPost("workflow")]
        public async Task<ActionResult<Response<bool>>> CheckWorkFlow([FromBody] ApprovalDto data)
        {
            var result = await _quotationService.CheckWorkFlow(data);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200
            return BadRequest(result);
        }
        [ClaimRequirement("Permission", new string[] { Permissions.QuotationClaims.Create, Permissions.QuotationClaims.View, Permissions.QuotationClaims.Delete, Permissions.QuotationClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<QuotationDto>>> GetByRequisitionId(int id)
        {
            var result = await _quotationService.GetRequisitionById(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }
    }
}
