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

    public class ReceiptController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IFileuploadServices _fileUploadService;
        public ReceiptController(IPaymentService paymentService, IFileuploadServices fileUploadService)
        {
            _paymentService = paymentService;
            _fileUploadService = fileUploadService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.ReceiptClaims.Create, Permissions.ReceiptClaims.View, Permissions.ReceiptClaims.Delete, Permissions.ReceiptClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<PaymentDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var payments = await _paymentService.GetAllAsync(filter, DocType.Receipt);
            if (payments.IsSuccess)
                return Ok(payments); // Status Code : 200

            return BadRequest(payments); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.ReceiptClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<PaymentDto>>> CreateAsync(CreatePaymentDto entity)
        {
            entity.PaymentType = PaymentType.Inflow;
            entity.PaymentFormType = DocType.Receipt;
            var payment = await _paymentService.CreateAsync(entity);
            if (payment.IsSuccess)
                return Ok(payment); // Status Code : 200

            return BadRequest(payment); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.ReceiptClaims.Create, Permissions.ReceiptClaims.View, Permissions.ReceiptClaims.Delete, Permissions.ReceiptClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<PaymentDto>>> GetByIdAsync(int id)
        {
            var result = await _paymentService.GetByIdAsync(id, DocType.Receipt);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.ReceiptClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<PaymentDto>>> UpdateAsync(int id, CreatePaymentDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            entity.PaymentType = PaymentType.Inflow;
            entity.PaymentFormType = DocType.Receipt;

            var result = await _paymentService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpPost("workflow")]
        public async Task<ActionResult<Response<bool>>> CheckWorkFlow([FromBody] ApprovalDto data)
        {
            var result = await _paymentService.CheckWorkFlow(data);
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
                    var result = await _fileUploadService.UploadFile(file, id, DocType.Receipt);
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
