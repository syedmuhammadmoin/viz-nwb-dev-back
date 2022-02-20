using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditNoteController : ControllerBase
    {
        private readonly ICreditNoteService _creditNoteService;

        public CreditNoteController(ICreditNoteService creditNoteService)
        {
            _creditNoteService = creditNoteService;
        }

        [HttpPost]
        public async Task<ActionResult<CreditNoteDto>> CreateAsync(CreateCreditNoteDto entity)
        {
            var CreditNote = await _creditNoteService.CreateAsync(entity);
            return Ok(CreditNote);
        }

        [HttpGet]
        public async Task<ActionResult<List<CreditNoteDto>>> GetAllAsync([FromQuery] PaginationFilter filter)
        {
            var invs = await _creditNoteService.GetAllAsync(filter);
            return Ok(invs);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CreditNoteDto>> GetByIdAsync(int id)
        {
            var result = await _creditNoteService.GetByIdAsync(id);
            return Ok(result); // Status Code : 200
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CreditNoteDto>> UpdateAsync(int id, CreateCreditNoteDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _creditNoteService.UpdateAsync(entity);
            return Ok(result); // Status Code : 200
        }
    }
}
