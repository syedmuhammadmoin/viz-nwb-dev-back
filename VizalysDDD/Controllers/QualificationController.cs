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
    public class QualificationController : ControllerBase
    {
        private readonly IQualificationService _qualificationService;

        public QualificationController(IQualificationService qualificationService)
        {
            _qualificationService = qualificationService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.QualificationClaims.Create, Permissions.QualificationClaims.View, Permissions.QualificationClaims.Delete, Permissions.QualificationClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<QualificationDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var results = await _qualificationService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.QualificationClaims.Create, Permissions.QualificationClaims.View, Permissions.QualificationClaims.Delete, Permissions.QualificationClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<QualificationDto>> GetByIdAsync(int id)
        {
            var result = await _qualificationService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpGet("Dropdown")]
        public async Task<ActionResult<Response<List<QualificationDto>>>> GetDropDown()
        {
            var result = await _qualificationService.GetDropDown();
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.QualificationClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<QualificationDto>>> CreateAsync(QualificationDto entity)
        {
            var result = await _qualificationService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.QualificationClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<QualificationDto>>> UpdateAsync(int id, QualificationDto entity)
        {
            if (id != entity.Id)
                return BadRequest("Id mismatch");

            var result = await _qualificationService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

    }
}
