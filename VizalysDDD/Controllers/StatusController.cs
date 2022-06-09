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
    public class StatusController : ControllerBase
    {
        private readonly IWorkFlowStatusService _workFlowStatusService;

        public StatusController(IWorkFlowStatusService workFlowStatusService)
        {
            _workFlowStatusService = workFlowStatusService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.WorkflowStatusClaims.Create, Permissions.WorkflowStatusClaims.View, Permissions.WorkflowStatusClaims.Delete, Permissions.WorkflowStatusClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<WorkFlowStatusDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var campuses = await _workFlowStatusService.GetAllAsync(filter);
            if (campuses.IsSuccess)
                return Ok(campuses); // Status Code : 200

            return BadRequest(campuses); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.WorkflowStatusClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<WorkFlowStatusDto>>> CreateAsync(CreateWorkFlowStatusDto entity)
        {
            var campus = await _workFlowStatusService.CreateAsync(entity);
            if (campus.IsSuccess)
                return Ok(campus); // Status Code : 200

            return BadRequest(campus); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.WorkflowStatusClaims.View, Permissions.WorkflowStatusClaims.Delete, Permissions.WorkflowStatusClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<WorkFlowStatusDto>>> GetByIdAsync(int id)
        {
            var result = await _workFlowStatusService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.WorkflowStatusClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<WorkFlowStatusDto>>> UpdateAsync(int id, CreateWorkFlowStatusDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _workFlowStatusService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpGet("Dropdown")]
        public async Task<ActionResult<Response<List<WorkFlowStatusDto>>>> GetStatusDropDown()
        {
            return Ok(await _workFlowStatusService.GetStatusDropDown()); // Status Code : 200
        }

    }
}
