using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JournalEntryController : ControllerBase
    {
        private readonly IJournalEntryService _journalEntryService;

        public JournalEntryController(IJournalEntryService journalEntryService)
        {
            _journalEntryService = journalEntryService;
        }

        [HttpPost]
        public async Task<ActionResult<Response<JournalEntryDto>>> CreateAsync(CreateJournalEntryDto entity)
        {
            var journalEntry = await _journalEntryService.CreateAsync(entity);
            if (journalEntry.IsSuccess)
                return Ok(journalEntry); // Status Code : 200

            return BadRequest(journalEntry); // Status code : 400
        }

        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<JournalEntryDto>>>> GetAllAsync([FromQuery] PaginationFilter filter)
        {
            var jvs = await _journalEntryService.GetAllAsync(filter);
            if (jvs.IsSuccess)
                return Ok(jvs); // Status Code : 200

            return BadRequest(jvs); // Status code : 400
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<JournalEntryDto>>> GetByIdAsync(int id)
        {
            var result = await _journalEntryService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

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

    }
}
