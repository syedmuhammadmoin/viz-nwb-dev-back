using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
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
        public async Task<ActionResult<List<OrganizationDto>>> GetAllAsync([FromQuery] PaginationFilter filter)
        {
            var orgs = await _organizationService.GetAllAsync(filter);
            return Ok(orgs);
        }

        [HttpPost]
        public async Task<ActionResult<OrganizationDto>> Create(CreateOrganizationDto entity)
        {
            var organizations = await _organizationService.CreateAsync(entity);
            return Ok(organizations);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrganizationDto>> Create(int id)
        {
            var result = await _organizationService.GetByIdAsync(id);
            return Ok(result); // Status Code : 200
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<OrganizationDto>> UpdateAsync(int id, CreateOrganizationDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _organizationService.UpdateAsync(entity);
            return Ok(result); // Status Code : 200
        }
    }
}
