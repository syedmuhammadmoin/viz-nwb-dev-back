using Application.Contracts.DTOs;
using Application.Contracts.Helper;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
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

        [ClaimRequirement("Permission", new string[] { Permissions.QuotationClaims.Create, Permissions.QuotationClaims.View, Permissions.QuotationClaims.Delete, Permissions.QuotationClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<QuotationDto>>> GetByIdAsync(int id)
        {
            var result =  _quotationComparativeService.GetRequisitionById(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }
    }
}
