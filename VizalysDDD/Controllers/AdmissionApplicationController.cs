using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Helper;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Domain.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdmissionApplicationController : ControllerBase
    {
        private readonly IAdmissionApplicationService _admissionApplicationService;

        public AdmissionApplicationController(IAdmissionApplicationService admissionApplicationService)
        {
            _admissionApplicationService = admissionApplicationService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.AdmissionApplicationClaims.Create, Permissions.AdmissionApplicationClaims.View, Permissions.AdmissionApplicationClaims.Delete, Permissions.AdmissionApplicationClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<AdmissionApplicationDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var results = await _admissionApplicationService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.AdmissionApplicationClaims.Create, Permissions.AdmissionApplicationClaims.View, Permissions.AdmissionApplicationClaims.Delete, Permissions.AdmissionApplicationClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<AdmissionApplicationDto>> GetByIdAsync(int id)
        {
            var result = await _admissionApplicationService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.AdmissionApplicationClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<AdmissionApplicationDto>>> CreateAsync(CreateAdmissionApplicationDto entity)
        {
            var result = await _admissionApplicationService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.AdmissionApplicationClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<AdmissionApplicationDto>>> UpdateAsync(int id, CreateAdmissionApplicationDto entity)
        {
            if (id != entity.Id)
                return BadRequest("Id mismatch");

            var result = await _admissionApplicationService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

    }
}
