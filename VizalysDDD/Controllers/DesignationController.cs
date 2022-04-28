using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Helper;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DesignationController : ControllerBase
    {
        private readonly IDesignationService _departmentService;

        public DesignationController(IDesignationService departmentService)
        {
            _departmentService = departmentService;
        }
        [ClaimRequirement("Permission", new string[] { Permissions.DesignationClaims.Create, Permissions.DesignationClaims.View, Permissions.DesignationClaims.Delete, Permissions.DesignationClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<DesignationDto>>>> GetAllAsync([FromQuery] PaginationFilter filter)
        {
            var departments = await _departmentService.GetAllAsync(filter);
            if (departments.IsSuccess)
                return Ok(departments); // Status Code : 200

            return BadRequest(departments); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.DesignationClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<DesignationDto>>> CreateAsync(DesignationDto entity)
        {
            var department = await _departmentService.CreateAsync(entity);
            if (department.IsSuccess)
                return Ok(department); // Status Code : 200

            return BadRequest(department); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.DesignationClaims.View, Permissions.DesignationClaims.Delete, Permissions.DesignationClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<DesignationDto>>> GetByIdAsync(int id)
        {
            var result = await _departmentService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpGet("Dropdown")]
        public async Task<ActionResult<Response<List<DesignationDto>>>> GetDesignationDropDown()
        {
            return Ok(await _departmentService.GetDesignationDropDown()); // Status Code : 200
        }
    }
}
