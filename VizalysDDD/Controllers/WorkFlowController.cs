using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Helper;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Domain.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkFlowController : ControllerBase
    {
        private readonly IWorkFlowService _workFlowService;

        public WorkFlowController(IWorkFlowService workFlowService)
        {
            _workFlowService = workFlowService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.WorkflowClaims.Create, Permissions.WorkflowClaims.View, Permissions.WorkflowClaims.Delete, Permissions.WorkflowClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<WorkFlowDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var campuses = await _workFlowService.GetAllAsync(filter);
            if (campuses.IsSuccess)
                return Ok(campuses); // Status Code : 200

            return BadRequest(campuses); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.WorkflowClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<WorkFlowDto>>> CreateAsync(CreateWorkFlowDto entity)
        {
            var campus = await _workFlowService.CreateAsync(entity);
            if (campus.IsSuccess)
                return Ok(campus); // Status Code : 200

            return BadRequest(campus); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.WorkflowClaims.View, Permissions.WorkflowClaims.Delete, Permissions.WorkflowClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<WorkFlowDto>>> GetByIdAsync(int id)
        {
            var result = await _workFlowService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.WorkflowClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<WorkFlowDto>>> UpdateAsync(int id, CreateWorkFlowDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _workFlowService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        //[HttpGet("Dropdown")]
        //public async Task<ActionResult<Response<List<WorkFlowDto>>>> GetStatusDropDown()
        //{
        //    return Ok(await _workFlowService.GetStatusDropDown()); // Status Code : 200
        //}

    }
}
