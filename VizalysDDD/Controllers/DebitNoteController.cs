using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DebitNoteController : ControllerBase
    {
        private readonly IDebitNoteService _debitNoteService;

        public DebitNoteController(IDebitNoteService debitNoteService)
        {
            _debitNoteService = debitNoteService;
        }

        [HttpPost]
        public async Task<ActionResult<DebitNoteDto>> CreateAsync(CreateDebitNoteDto entity)
        {
            var DebitNote = await _debitNoteService.CreateAsync(entity);
            return Ok(DebitNote);
        }

        [HttpGet]
        public async Task<ActionResult<List<DebitNoteDto>>> GetAllAsync([FromQuery] PaginationFilter filter)
        {
            var invs = await _debitNoteService.GetAllAsync(filter);
            return Ok(invs);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<DebitNoteDto>> GetByIdAsync(int id)
        {
            var result = await _debitNoteService.GetByIdAsync(id);
            return Ok(result); // Status Code : 200
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<DebitNoteDto>> UpdateAsync(int id, CreateDebitNoteDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _debitNoteService.UpdateAsync(entity);
            return Ok(result); // Status Code : 200
        }
    }
}
