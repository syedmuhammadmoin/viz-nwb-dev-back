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
    public class JournalEntryController : ControllerBase
    {
        private readonly IJournalEntryService _journalEntryService;
        private readonly IFileuploadServices _fileUploadService;
        public JournalEntryController(IJournalEntryService journalEntryService, IFileuploadServices fileUploadService)
        {
            _journalEntryService = journalEntryService;
            _fileUploadService = fileUploadService;

        }

        [ClaimRequirement("Permission", new string[] { Permissions.JournalEntryClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<JournalEntryDto>>> CreateAsync(CreateJournalEntryDto entity)
        {
            var journalEntry = await _journalEntryService.CreateAsync(entity);
            if (journalEntry.IsSuccess)
                return Ok(journalEntry); // Status Code : 200

            return BadRequest(journalEntry); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.JournalEntryClaims.Create, Permissions.JournalEntryClaims.View, Permissions.JournalEntryClaims.Delete, Permissions.JournalEntryClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<JournalEntryDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var jvs = await _journalEntryService.GetAllAsync(filter);
            if (jvs.IsSuccess)
                return Ok(jvs); // Status Code : 200

            return BadRequest(jvs); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.JournalEntryClaims.Create, Permissions.JournalEntryClaims.View, Permissions.JournalEntryClaims.Delete, Permissions.JournalEntryClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<JournalEntryDto>>> GetByIdAsync(int id)
        {
            var result = await _journalEntryService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.JournalEntryClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<JournalEntryDto>>> UpdateAsync(int id, CreateJournalEntryDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _journalEntryService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpPost("workflow")]
        public async Task<ActionResult<Response<bool>>> CheckWorkFlow([FromBody] ApprovalDto data)
        {
            var result = await _journalEntryService.CheckWorkFlow(data);
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
                    var result = await _fileUploadService.UploadFile(file, id, DocType.JournalEntry);
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
