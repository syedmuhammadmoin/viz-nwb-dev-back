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
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService;

        private readonly IFileuploadServices _fileUploadService;

        public RequestController(IRequestService requestService, IFileuploadServices fileUploadService)
        {
            _requestService = requestService;
            _fileUploadService = fileUploadService;
        }
        [ClaimRequirement("Permission", new string[] { Permissions.RequestClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<RequestDto>>> CreateAsync(CreateRequestDto entity)
        {
            var result = await _requestService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }
        [ClaimRequirement("Permission", new string[] { Permissions.RequestClaims.Create, Permissions.RequestClaims.View, Permissions.RequestClaims.Delete, Permissions.RequestClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<RequestDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var results = await _requestService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }
        [ClaimRequirement("Permission", new string[] { Permissions.RequestClaims.Create, Permissions.RequestClaims.View, Permissions.RequestClaims.Delete, Permissions.RequestClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<RequestDto>>> GetByIdAsync(int id)
        {
            var result = await _requestService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }
        [ClaimRequirement("Permission", new string[] { Permissions.RequestClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<RequestDto>>> UpdateAsync(int id, CreateRequestDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _requestService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpPost("workflow")]
        public async Task<ActionResult<Response<bool>>> CheckWorkFlow([FromBody] ApprovalDto data)
        {
            var result = await _requestService.CheckWorkFlow(data);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200
            return BadRequest(result);
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
