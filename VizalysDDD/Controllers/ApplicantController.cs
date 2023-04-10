using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Helper;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicantController : ControllerBase
    {
        private readonly IApplicantService _applicantService;

        public ApplicantController(IApplicantService applicantService)
        {
            _applicantService = applicantService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.ApplicantClaims.Create, Permissions.ApplicantClaims.View, Permissions.ApplicantClaims.Delete, Permissions.ApplicantClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<AcademicDepartmentDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var results = await _applicantService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.ApplicantClaims.Create, Permissions.ApplicantClaims.View, Permissions.ApplicantClaims.Delete, Permissions.ApplicantClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<AcademicDepartmentDto>> GetByIdAsync(int id)
        {
            var result = await _applicantService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpGet("Dropdown")]
        public async Task<ActionResult<Response<List<AcademicDepartmentDto>>>> GetDropDown()
        {
            var result = await _applicantService.GetDropDown();
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        //[ClaimRequirement("Permission", new string[] { Permissions.ApplicantClaims.Create })]
        //[HttpPost]
        //public async Task<ActionResult<Response<AcademicDepartmentDto>>> CreateAsync(CreateAcademicDepartmentDto entity)
        //{
        //    var result = await _applicantService.CreateAsync(entity);
        //    if (result.IsSuccess)
        //        return Ok(result); // Status Code : 200

        //    return BadRequest(result); // Status code : 400
        //}

        //[ClaimRequirement("Permission", new string[] { Permissions.ApplicantClaims.Edit })]
        //[HttpPut("{id:int}")]
        //public async Task<ActionResult<Response<AcademicDepartmentDto>>> UpdateAsync(int id, CreateAcademicDepartmentDto entity)
        //{
        //    if (id != entity.Id)
        //        return BadRequest("Id mismatch");

        //    var result = await _applicantService.UpdateAsync(entity);
        //    if (result.IsSuccess)
        //        return Ok(result); // Status Code : 200

        //    return BadRequest(result); // Status code : 400
        //}

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<Response<string>>> LoginApplicant(LoginDto entity)
        {
            var result = await _applicantService.LoginApplicant(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }


        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<ActionResult<Response<int>>> CreateAsync(RegisterApplicantDto entity)
        {
            var result = await _applicantService.RegisterApplicant(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

    }
}
