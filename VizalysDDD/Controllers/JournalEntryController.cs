using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
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
        public async Task<ActionResult<JournalEntryDto>> CreateAsync(CreateJournalEntryDto entity)
        {
            var journalEntry = await _journalEntryService.CreateAsync(entity);
            return Ok(journalEntry);
        }

        [HttpGet]
        public async Task<ActionResult<List<JournalEntryDto>>> GetAllAsync([FromQuery] PaginationFilter filter)
        {
            var jvs = await _journalEntryService.GetAllAsync(filter);
            return Ok(jvs);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<JournalEntryDto>> GetByIdAsync(int id)
        {
            var result = await _journalEntryService.GetByIdAsync(id);
            return Ok(result); // Status Code : 200
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<JournalEntryDto>> UpdateAsync(int id, CreateJournalEntryDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _journalEntryService.UpdateAsync(entity);
            return Ok(result); // Status Code : 200
        }

    }
}
