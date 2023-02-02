using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Helper;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Application.Services;
using Domain.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CWIPController : ControllerBase
    {
        private readonly ICWIPService _cWIPService;

        public CWIPController(ICWIPService cWIPService)
        {
            _cWIPService = cWIPService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.CWIPClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<CWIPDto>>> CreateAsync(CreateCWIPDto entity)
        {
            var result = await _cWIPService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.CWIPClaims.Create, Permissions.CWIPClaims.View, Permissions.CWIPClaims.Delete, Permissions.CWIPClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<CWIPDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var results = await _cWIPService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }
        [ClaimRequirement("Permission", new string[] { Permissions.CWIPClaims.Create, Permissions.CWIPClaims.View, Permissions.CWIPClaims.Delete, Permissions.CWIPClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<CWIPDto>>> GetByIdAsync(int id)
        {
            var result = await _cWIPService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }
        [ClaimRequirement("Permission", new string[] { Permissions.CWIPClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<CWIPDto>>> UpdateAsync(int id, CreateCWIPDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _cWIPService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }
        [HttpPost("workflow")]
        public async Task<ActionResult<Response<bool>>> CheckWorkFlow([FromBody] ApprovalDto data)
        {
            var result = await _cWIPService.CheckWorkFlow(data);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status Code : 400
        }
    }
}
