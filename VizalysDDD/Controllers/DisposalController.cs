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
    public class DisposalController : ControllerBase
    {
        private readonly IDisposalService _disposalService;

        public DisposalController(IDisposalService disposalService)
        {
            _disposalService = disposalService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.DisposalClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<DisposalDto>>> CreateAsync(CreateDisposalDto entity)
        {
            var result = await _disposalService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }
        
        [ClaimRequirement("Permission", new string[] { Permissions.DisposalClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<DisposalDto>>> UpdateAsync(int id, CreateDisposalDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _disposalService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.DisposalClaims.Create, Permissions.DisposalClaims.View, Permissions.DisposalClaims.Delete, Permissions.DisposalClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<DisposalDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        
        {
            var results = await _disposalService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.DisposalClaims.Create, Permissions.DisposalClaims.View, Permissions.DisposalClaims.Delete, Permissions.DisposalClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<DisposalDto>>> GetByIdAsync(int id)
        {
            var result = await _disposalService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }
       
        [HttpPost("workflow")]
        public async Task<ActionResult<Response<bool>>> CheckWorkFlow([FromBody] ApprovalDto data)
        {
            var result = await _disposalService.CheckWorkFlow(data);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status Code : 400
        }

    }
}
