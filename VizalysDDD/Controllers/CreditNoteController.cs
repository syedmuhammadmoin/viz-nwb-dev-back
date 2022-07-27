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
    public class CreditNoteController : ControllerBase
    {
        private readonly ICreditNoteService _creditNoteService;
        private readonly IFileuploadServices _fileuploadServices;
        public CreditNoteController(ICreditNoteService creditNoteService, IFileuploadServices fileuploadServices)
        {
            _creditNoteService = creditNoteService;
            _fileuploadServices = fileuploadServices;

        }

        [ClaimRequirement("Permission", new string[] { Permissions.CreditNoteClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<CreditNoteDto>>> CreateAsync(CreateCreditNoteDto entity)
        {
            var result = await _creditNoteService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.CreditNoteClaims.Create, Permissions.CreditNoteClaims.View, Permissions.CreditNoteClaims.Delete, Permissions.CreditNoteClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<CreditNoteDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var results = await _creditNoteService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.CreditNoteClaims.Create, Permissions.CreditNoteClaims.View, Permissions.CreditNoteClaims.Delete, Permissions.CreditNoteClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<CreditNoteDto>>> GetByIdAsync(int id)
        {
            var result = await _creditNoteService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.CreditNoteClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<CreditNoteDto>>> UpdateAsync(int id, CreateCreditNoteDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _creditNoteService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpPost("workflow")]
        public async Task<ActionResult<Response<bool>>> CheckWorkFlow([FromBody] ApprovalDto data)
        {
            var result = await _creditNoteService.CheckWorkFlow(data);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200
            return BadRequest(result);
        }
        [HttpPost("DocUpload/{id:int}")]
        public async Task<ActionResult<bool>> UploadFile(IFormFile file, int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _fileuploadServices.UploadFile(file, id, DocType.CreditNote); ;
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
