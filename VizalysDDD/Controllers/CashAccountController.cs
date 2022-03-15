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
    public class CashAccountController : ControllerBase
    {

        private readonly ICashAccountService _cashAccountService;

        public CashAccountController(ICashAccountService cashAccountService)
        {
            _cashAccountService = cashAccountService;
        }
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<CashAccountDto>>>> GetAllAsync([FromQuery] PaginationFilter filter)
        {
            return Ok(await _cashAccountService.GetAllAsync(filter)); // Status Code : 200
        }

        [HttpPost]
        public async Task<ActionResult<Response<CashAccountDto>>> CreateAsync(CreateCashAccountDto entity)
        {
            var cashAccount = await _cashAccountService.CreateAsync(entity);
            if (cashAccount.IsSuccess)
                return Ok(cashAccount); // Status Code : 200

            return BadRequest(cashAccount); // Status code : 400
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<CashAccountDto>>> GetByIdAsync(int id)
        {
            var result = await _cashAccountService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<CashAccountDto>>> UpdateAsync(int id, UpdateCashAccountDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _cashAccountService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpGet("Dropdown")]
        public async Task<ActionResult<Response<List<CategoryDto>>>> GetCashAccountDropDown()
        {
            return Ok(await _cashAccountService.GetCashAccountDropDown()); // Status Code : 200
        }
    }
}
