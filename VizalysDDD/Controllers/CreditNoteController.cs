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
    public class CreditNoteController : ControllerBase
    {
        private readonly ICreditNoteService _creditNoteService;

        public CreditNoteController(ICreditNoteService creditNoteService)
        {
            _creditNoteService = creditNoteService;
        }

        [HttpPost]
        public async Task<ActionResult<Response<CreditNoteDto>>> CreateAsync(CreateCreditNoteDto entity)
        {
            var result = await _creditNoteService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<CreditNoteDto>>>> GetAllAsync([FromQuery] PaginationFilter filter)
        {
            var results = await _creditNoteService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<CreditNoteDto>>> GetByIdAsync(int id)
        {
            var result = await _creditNoteService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

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
    }
}
