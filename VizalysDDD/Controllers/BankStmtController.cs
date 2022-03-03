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
    public class BankStmtController : ControllerBase
    {
        private readonly IBankStmtService _bankStmtService;

        public BankStmtController(IBankStmtService bankStmtService)
        {
            _bankStmtService = bankStmtService;
        }
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<BankStmtDto>>>> GetAllAsync([FromQuery] PaginationFilter filter)
        {
            return Ok(await _bankStmtService.GetAllAsync(filter)); // Status Code : 200
        }

        [HttpPost]
        public async Task<ActionResult<Response<BankStmtDto>>> CreateAsync(CreateBankStmtDto entity)
        {
            var bankStmt = await _bankStmtService.CreateAsync(entity);
            if (bankStmt.IsSuccess)
                return Ok(bankStmt); // Status Code : 200

            return BadRequest(bankStmt); // Status code : 400
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<BankStmtDto>>> GetByIdAsync(int id)
        {
            var result = await _bankStmtService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<BankStmtDto>>> UpdateAsync(int id, CreateBankStmtDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _bankStmtService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

    }
}
