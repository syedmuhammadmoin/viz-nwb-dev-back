using Application.Contracts.DTOs;
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

    }
}
