
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


    public class CallForQuotationController : ControllerBase
    {
        private readonly ICallForQuotationService _callForQuotationService;
        private readonly IFileuploadServices _fileUploadService;

        public CallForQuotationController(ICallForQuotationService callForQuotationService, IFileuploadServices fileuploadServices)
        {
            _callForQuotationService = callForQuotationService;
            _fileUploadService = fileuploadServices;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.CallForQuotationClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<CallForQuotationDto>>> CreateAsync(CreateCallForQuotationDto entity)
        {
            var result = await _callForQuotationService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }
        [ClaimRequirement("Permission", new string[] { Permissions.CallForQuotationClaims.Create, Permissions.CallForQuotationClaims.View, Permissions.CallForQuotationClaims.Delete, Permissions.CallForQuotationClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<CallForQuotationDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var result = await _callForQuotationService.GetAllAsync(filter);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }
        [ClaimRequirement("Permission", new string[] { Permissions.CallForQuotationClaims.Create, Permissions.CallForQuotationClaims.View, Permissions.CallForQuotationClaims.Delete, Permissions.CallForQuotationClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<CallForQuotationDto>>> GetByIdAsync(int id)
        {
            var result = await _callForQuotationService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }
        [ClaimRequirement("Permission", new string[] { Permissions.CallForQuotationClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<CallForQuotationDto>>> UpdateAsync(int id, CreateCallForQuotationDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _callForQuotationService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }
        [HttpPost("DocUpload/{id:int}")]
        public async Task<ActionResult<Response<int>>> UploadFile(IFormFile file, int id)
        {
            if (ModelState.IsValid)
            {
                var result = await _fileUploadService.UploadFile(file, id, DocType.Quotation);
                if (result.IsSuccess)
                    return Ok(result); // Status Code : 200
                return BadRequest(result);
            }
            return BadRequest("Some properties are not valid"); // Status code : 400
        }
    }
}
