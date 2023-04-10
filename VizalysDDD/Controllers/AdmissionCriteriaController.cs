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
    public class AdmissionCriteriaController : ControllerBase
    {
        private readonly IAdmissionCriteriaService _admissionCriteriaService;

        public AdmissionCriteriaController(IAdmissionCriteriaService admissionCriteriaService)
        {
            _admissionCriteriaService = admissionCriteriaService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.AdmissionCriteriaClaims.Create, Permissions.AdmissionCriteriaClaims.View, Permissions.AdmissionCriteriaClaims.Delete, Permissions.AdmissionCriteriaClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<AdmissionCriteriaDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var results = await _admissionCriteriaService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.AdmissionCriteriaClaims.Create, Permissions.AdmissionCriteriaClaims.View, Permissions.AdmissionCriteriaClaims.Delete, Permissions.AdmissionCriteriaClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<AdmissionCriteriaDto>> GetByIdAsync(int id)
        {
            var result = await _admissionCriteriaService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.AdmissionCriteriaClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<AdmissionCriteriaDto>>> CreateAsync(CreateAdmissionCriteriaDto entity)
        {
            var result = await _admissionCriteriaService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.AdmissionCriteriaClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<AdmissionCriteriaDto>>> UpdateAsync(int id, CreateAdmissionCriteriaDto entity)
        {
            if (id != entity.Id)
                return BadRequest("Id mismatch");

            var result = await _admissionCriteriaService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

    }
}
