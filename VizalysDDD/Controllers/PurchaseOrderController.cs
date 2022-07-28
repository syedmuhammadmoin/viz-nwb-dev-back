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
        private readonly IFileuploadServices _fileUploadService;
        public PurchaseOrderController(IPurchaseOrderService purchaseOrderService, IFileuploadServices fileUploadService)
        {
            _purchaseOrderService = purchaseOrderService;
            _fileUploadService = fileUploadService; 

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
        public async Task<ActionResult<PaginationResponse<List<PurchaseOrderDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var results = await _purchaseOrderService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.PurchaseOrderClaims.Create, Permissions.PurchaseOrderClaims.View, Permissions.PurchaseOrderClaims.Delete, Permissions.PurchaseOrderClaims.Edit })]
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

        [HttpPost("workflow")]
        public async Task<ActionResult<Response<bool>>> CheckWorkFlow([FromBody] ApprovalDto data)
        {
            var result = await _purchaseOrderService.CheckWorkFlow(data);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200
            return BadRequest(result);
         
        }
        [HttpPost("DocUpload/{id:int}")]
        public async Task<ActionResult<Response<int>>> UploadFile(IFormFile file, int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _fileUploadService.UploadFile(file, id, DocType.PurchaseOrder);
                    if (result.IsSuccess)
                        return Ok(result); // Status Code : 200
                    return BadRequest(result);
                }
                return BadRequest("Some properties are not valid"); // Status code : 400
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.Message);
            }
        }
    }
}
