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
    public class BidEvaluationController : ControllerBase
    {
        private readonly IBidEvaluationService _bidEvaluationService;

        public BidEvaluationController(IBidEvaluationService bidEvaluationService)
        {
            _bidEvaluationService = bidEvaluationService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.BidEvaluationClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<BidEvaluationDto>>> CreateAsync(CreateBidEvaluationDto entity)
        {
            var bidEvaluation = await _bidEvaluationService.CreateAsync(entity);
            if (bidEvaluation.IsSuccess)
                return Ok(bidEvaluation); // Status Code : 200

            return BadRequest(bidEvaluation); // Status code : 400
        }
        [ClaimRequirement("Permission", new string[] { Permissions.BidEvaluationClaims.Create, Permissions.BidEvaluationClaims.View, Permissions.BidEvaluationClaims.Delete, Permissions.BidEvaluationClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<BidEvaluationDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var bidEvaluation = await _bidEvaluationService.GetAllAsync(filter);
            if (bidEvaluation.IsSuccess)
                return Ok(bidEvaluation); // Status Code : 200

            return BadRequest(bidEvaluation); // Status code : 400
        }
        [ClaimRequirement("Permission", new string[] { Permissions.BidEvaluationClaims.Create, Permissions.BidEvaluationClaims.View, Permissions.BidEvaluationClaims.Delete, Permissions.BidEvaluationClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<BidEvaluationDto>>> GetByIdAsync(int id)
        {
            var result = await _bidEvaluationService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }
        [ClaimRequirement("Permission", new string[] { Permissions.BillClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<BidEvaluationDto>>> UpdateAsync(int id, CreateBidEvaluationDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _bidEvaluationService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }
    }
}
