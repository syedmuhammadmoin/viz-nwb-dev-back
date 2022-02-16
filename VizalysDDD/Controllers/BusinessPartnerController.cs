using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessPartnerController : ControllerBase
    {
        private readonly IBusinessPartnerService _businessPartnerService;

        public BusinessPartnerController(IBusinessPartnerService businessPartnerService)
        {
            _businessPartnerService = businessPartnerService;
        }
        [HttpGet]
        public async Task<ActionResult<List<BusinessPartnerDto>>> GetAllAsync([FromQuery] PaginationFilter filter)
        {
            var businessPartner = await _businessPartnerService.GetAllAsync(filter);
            return Ok(businessPartner);
        }

        [HttpPost]
        public async Task<ActionResult<BusinessPartnerDto>> CreateAsync(CreateBusinessPartnerDto entity)
        {
            var businessPartner = await _businessPartnerService.CreateAsync(entity);
            return Ok(businessPartner);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BusinessPartnerDto>> GetByIdAsync(int id)
        {
            var result = await _businessPartnerService.GetByIdAsync(id);
            return Ok(result); // Status Code : 200
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<BusinessPartnerDto>> UpdateAsync(int id, CreateBusinessPartnerDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _businessPartnerService.UpdateAsync(entity);
            return Ok(result); // Status Code : 200
        }

    }
}
