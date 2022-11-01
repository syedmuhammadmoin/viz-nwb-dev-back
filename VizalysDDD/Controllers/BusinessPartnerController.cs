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
    public class BusinessPartnerController : ControllerBase
    {
        private readonly IBusinessPartnerService _businessPartnerService;

        public BusinessPartnerController(IBusinessPartnerService businessPartnerService)
        {
            _businessPartnerService = businessPartnerService;
        }
        
        [ClaimRequirement("Permission", new string[] { Permissions.BusinessPartnerClaims.Create, Permissions.BusinessPartnerClaims.View, Permissions.BusinessPartnerClaims.Delete, Permissions.BusinessPartnerClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<BusinessPartnerDto>>>> GetAllAsync([FromQuery] BusinessPartnerFilter filter)
        {
            var businessPartners = await _businessPartnerService.GetAllAsync(filter);
            if (businessPartners.IsSuccess)
                return Ok(businessPartners); // Status Code : 200

            return BadRequest(businessPartners); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.BusinessPartnerClaims.Create,})]
        [HttpPost]
        public async Task<ActionResult<Response<BusinessPartnerDto>>> CreateAsync(CreateBusinessPartnerDto entity)
        {
            var businessPartner = await _businessPartnerService.CreateAsync(entity);
            if (businessPartner.IsSuccess)
                return Ok(businessPartner); // Status Code : 200

            return BadRequest(businessPartner); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.BusinessPartnerClaims.Create, Permissions.BusinessPartnerClaims.View, Permissions.BusinessPartnerClaims.Delete, Permissions.BusinessPartnerClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<BusinessPartnerDto>>> GetByIdAsync(int id)
        {
            var result = await _businessPartnerService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.BusinessPartnerClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<BusinessPartnerDto>>> UpdateAsync(int id, CreateBusinessPartnerDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _businessPartnerService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpGet("Dropdown")]
        public async Task<ActionResult<Response<List<BusinessPartnerDto>>>> GetBusinessPartnerDropDown()
        {
            return Ok(await _businessPartnerService.GetBusinessPartnerDropDown()); // Status Code : 200
        }

        [HttpGet("AllBusinessPartnerDropdown")]
        public ActionResult<Response<List<EmployeeBusinessPartnerDto>>> GetAllBusinessPartnerDropDown()
        {
            return Ok(_businessPartnerService.GetAllBusinessPartnerDropDown()); // Status Code : 200
        }
    }
}
