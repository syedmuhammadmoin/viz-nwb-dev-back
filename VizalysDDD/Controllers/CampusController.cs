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
    public class CampusController : ControllerBase
    {
        private readonly ICampusService _campusService;

        public CampusController(ICampusService campusService)
        {
            _campusService = campusService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.CampusClaims.Create, Permissions.CampusClaims.View, Permissions.CampusClaims.Delete, Permissions.CampusClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<CampusDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var campuses = await _campusService.GetAllAsync(filter);
            if (campuses.IsSuccess)
                return Ok(campuses); // Status Code : 200

            return BadRequest(campuses); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.CampusClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<CampusDto>>> CreateAsync(CampusDto entity)
        {
            var campus = await _campusService.CreateAsync(entity);
            if (campus.IsSuccess)
                return Ok(campus); // Status Code : 200

            return BadRequest(campus); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.CampusClaims.Create, Permissions.CampusClaims.View, Permissions.CampusClaims.Delete, Permissions.CampusClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<CampusDto>>> GetByIdAsync(int id)
        {
            var result = await _campusService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.CampusClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<CampusDto>>> UpdateAsync(int id, CampusDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _campusService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpGet("Dropdown")]
        public async Task<ActionResult<Response<List<CampusDto>>>> GetCampusDropDown()
        {
            return Ok(await _campusService.GetCampusDropDown()); // Status Code : 200
        }
    }
}
