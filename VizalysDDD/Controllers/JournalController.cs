using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Helper;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Application.Services;
using Domain.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JournalController : Controller
    {
        private readonly IJournalService _journalService;
        public JournalController(IJournalService journalService)
        {
            _journalService = journalService;
        }
        [ClaimRequirement("Permission", new string[] { Permissions.JournalClaims.Create, Permissions.JournalClaims.View, Permissions.JournalClaims.Delete, Permissions.JournalClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<JournalDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var results = await _journalService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }
        [ClaimRequirement("Permission", new string[] { Permissions.JournalClaims.Create, Permissions.JournalClaims.View, Permissions.JournalClaims.Delete, Permissions.JournalClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<JournalDto>> GetByIdAsync(int id)
        {
            var result = await _journalService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.JournalClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<JournalDto>>> CreateAsync(CreateJournalDto entity)
        {
            var result = await _journalService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.JournalClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<JournalDto>>> UpdateAsync(int id, CreateJournalDto entity)
        {
            if (id != entity.Id)
                return BadRequest("Id mismatch");

            var result = await _journalService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }
        [HttpDelete]
        public async Task<ActionResult<Response<string>>> DeleteJournals(List<int> ids)
        {
            var result = await _journalService.DeleteJournals(ids);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }

    }
}
