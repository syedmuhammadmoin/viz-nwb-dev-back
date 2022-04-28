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
    public class PurchaseOrderController : ControllerBase
    {
        private readonly IPurchaseOrderService _purchaseOrderService;

        public PurchaseOrderController(IPurchaseOrderService purchaseOrderService)
        {
            _purchaseOrderService = purchaseOrderService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.PurchaseOrderClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<PurchaseOrderDto>>> CreateAsync(CreatePurchaseOrderDto entity)
        {
            var result = await _purchaseOrderService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.PurchaseOrderClaims.Create, Permissions.PurchaseOrderClaims.View, Permissions.PurchaseOrderClaims.Delete, Permissions.PurchaseOrderClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<PurchaseOrderDto>>>> GetAllAsync([FromQuery] PaginationFilter filter)
        {
            var results = await _purchaseOrderService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.PurchaseOrderClaims.View, Permissions.PurchaseOrderClaims.Delete, Permissions.PurchaseOrderClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<PurchaseOrderDto>>> GetByIdAsync(int id)
        {
            var result = await _purchaseOrderService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.PurchaseOrderClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<PurchaseOrderDto>>> UpdateAsync(int id, CreatePurchaseOrderDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _purchaseOrderService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.PurchaseOrderClaims.View })]
        [HttpPost("workflow")]
        public async Task<ActionResult<Response<bool>>> CheckWorkFlow([FromBody] ApprovalDto data)
        {
            var result = await _purchaseOrderService.CheckWorkFlow(data);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200
            return BadRequest(result);
        }
    }
}
