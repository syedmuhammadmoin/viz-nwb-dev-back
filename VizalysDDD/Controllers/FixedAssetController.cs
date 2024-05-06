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
    [Authorize]
    public class FixedAssetController : ControllerBase
    {
        private readonly IFixedAssetService _fixedAssetService;
        private readonly IFixedAssetReportService _fixedAssetReportService;

        public FixedAssetController(IFixedAssetService fixedAssetService, IFixedAssetReportService fixedAssetReportService)
        {
            _fixedAssetService = fixedAssetService;
            _fixedAssetReportService = fixedAssetReportService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.FixedAssetClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<FixedAssetDto>>> CreateAsync(CreateFixedAssetDto entity)
        {
            var result = await _fixedAssetService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.FixedAssetClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<FixedAssetDto>>> UpdateAsync(int id, UpdateFixedAssetDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _fixedAssetService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.FixedAssetClaims.Edit })]
        [HttpPut("Update/{id:int}")]
        public async Task<ActionResult<Response<FixedAssetDto>>> UpdateAfterApproval(int id, UpdateSalvageValueDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _fixedAssetService.UpdateAfterApproval(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }


        [ClaimRequirement("Permission", new string[] { Permissions.FixedAssetClaims.Create, Permissions.FixedAssetClaims.View, Permissions.FixedAssetClaims.Delete, Permissions.FixedAssetClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<FixedAssetDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var results = await _fixedAssetService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }
       
        [ClaimRequirement("Permission", new string[] { Permissions.FixedAssetClaims.Create, Permissions.FixedAssetClaims.View, Permissions.FixedAssetClaims.Delete, Permissions.FixedAssetClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<FixedAssetDto>>> GetByIdAsync(int id)
        {
            var result = await _fixedAssetService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }
        
        [HttpPost("workflow")]
        public async Task<ActionResult<Response<bool>>> CheckWorkFlow([FromBody] ApprovalDto data)
        {
            var result = await _fixedAssetService.CheckWorkFlow(data);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status Code : 400
        }

        [HttpGet("Dropdown")]
        public async Task<ActionResult<Response<List<Level4Dto>>>> GetDropDown()
        {
            var result = await _fixedAssetService.GetDropDown();
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status Code : 400
        }
        
        [HttpGet("Disposable/Dropdown")]
        public async Task<ActionResult<Response<List<Level4Dto>>>> GetDisposableAssetDropDown()
        {
            var result = await _fixedAssetService.GetDisposableAssetDropDown();
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status Code : 400
        }
        
        [HttpGet("Product/{ProductId:int}")]
        public async Task<ActionResult<Response<List<Level4Dto>>>> GetAssetByProductIdDropDown(int ProductId)
        {
            var result = await _fixedAssetService.GetAssetInStockByProductIdDropDown(ProductId);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status Code : 400
        }

        [HttpPost("HeldForDisposal")]
        public async Task<ActionResult<Response<bool>>> HeldAssetForDisposal([FromBody] CreateHeldAssetForDisposal data)
        {
            var result = await _fixedAssetService.HeldAssetForDisposal(data);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status Code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.FixedAssetReportClaims.View })]
        [HttpPost("Report")]
        public ActionResult<Response<List<FixedAssetReportDto>>> GetReport(FixedAssetReportFilter filters)
        {
            var result = _fixedAssetReportService.GetReport(filters);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status Code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.FixedAssetReportClaims.View })]
        [HttpPost("MonthlyReport")]
        public ActionResult<Response<List<FixedAssetReportDto>>> GetReportMonthly(FixedAssetReportFilter filters)
        {
            var result = _fixedAssetReportService.GetReportMonthly(filters);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status Code : 400
        }
        [ClaimRequirement("Permission", new string[] { Permissions.FixedAssetClaims.Create, Permissions.FixedAssetClaims.View, Permissions.FixedAssetClaims.Delete, Permissions.FixedAssetClaims.Edit })]
        [HttpGet("DepreciationSchedule/{fixedAssetId:int}")]
        public async Task<ActionResult<Response<bool>>> DepreciationSchedule(int fixedAssetId)
        {
            var result = await _fixedAssetService.DepreciationSchedule(fixedAssetId);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status Code : 400
        }

    }
}
