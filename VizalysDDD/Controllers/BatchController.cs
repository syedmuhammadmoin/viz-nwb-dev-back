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
    public class BatchController : ControllerBase
    {
        private readonly IBatchService _batchService;

        public BatchController(IBatchService batchService)
        {
            _batchService = batchService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.BatchClaims.Create, Permissions.BatchClaims.View, Permissions.BatchClaims.Delete, Permissions.BatchClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<BatchDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var results = await _batchService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.BatchClaims.Create, Permissions.BatchClaims.View, Permissions.BatchClaims.Delete, Permissions.BatchClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<BatchDto>> GetByIdAsync(int id)
        {
            var result = await _batchService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpGet("Dropdown")]
        public async Task<ActionResult<Response<List<BatchDto>>>> GetDropDown()
        {
            var result = await _batchService.GetDropDown();
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.BatchClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<BatchDto>>> CreateAsync(CreateBatchDto entity)
        {
            var result = await _batchService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.BatchClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<BatchDto>>> UpdateAsync(int id, CreateBatchDto entity)
        {
            if (id != entity.Id)
                return BadRequest("Id mismatch");

            var result = await _batchService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.BatchClaims.Edit })]
        [HttpPost("AddCriteria")]
        public async Task<ActionResult<Response<BatchDto>>> CreateAsync(AddCriteriaDto entity)
        {
            var result = await _batchService.AddCriteria(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

    }
}
