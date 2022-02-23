using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationService _organizationService;

        public OrganizationController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }

        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<OrganizationDto>>>> GetAllAsync([FromQuery] PaginationFilter filter)
        {
            var orgs = await _organizationService.GetAllAsync(filter);
            if (orgs.IsSuccess)
                return Ok(orgs); // Status Code : 200

            return BadRequest(orgs); // Status code : 400
        }

        [HttpPost]
        public async Task<ActionResult<Response<OrganizationDto>>> CreateAsync(CreateOrganizationDto entity)
        {
            var org = await _organizationService.CreateAsync(entity);
            if (org.IsSuccess)
                return Ok(org); // Status Code : 200

            return BadRequest(org); // Status code : 400
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<OrganizationDto>>> GetByIdAsync(int id)
        {
            var result = await _organizationService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<OrganizationDto>>> UpdateAsync(int id, CreateOrganizationDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _organizationService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }
    }
}
