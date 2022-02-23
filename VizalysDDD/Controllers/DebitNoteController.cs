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
    public class DebitNoteController : ControllerBase
    {
        private readonly IDebitNoteService _debitNoteService;

        public DebitNoteController(IDebitNoteService debitNoteService)
        {
            _debitNoteService = debitNoteService;
        }

        [HttpPost]
        public async Task<ActionResult<Response<DebitNoteDto>>> CreateAsync(CreateDebitNoteDto entity)
        {
            var result = await _debitNoteService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<DebitNoteDto>>>> GetAllAsync([FromQuery] PaginationFilter filter)
        {
            var results = await _debitNoteService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<DebitNoteDto>>> GetByIdAsync(int id)
        {
            var result = await _debitNoteService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<DebitNoteDto>>> UpdateAsync(int id, CreateDebitNoteDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _debitNoteService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }
    }
}
