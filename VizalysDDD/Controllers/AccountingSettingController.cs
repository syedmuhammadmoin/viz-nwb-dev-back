using Application.Contracts.DTOs.AccountSetting;
using Application.Contracts.DTOs.FiscalPeriod;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountingSettingController : ControllerBase
    {
        private readonly IAccountingSettingService _service;

        public AccountingSettingController(IAccountingSettingService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<List<AccountingSettingDto>>> GetAllAsync([FromQuery] TransactionFormFilter? filter)
        {
            var result = await _service.GetAllAsync(filter);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<AccountingSettingDto>> GetById(int id)
        {
            
            var result = await _service.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost]
        public async Task<ActionResult<AccountingSettingDto>> CreateAsync(CreateAccountingSettingDto entity)
        {
            var result = await _service.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<AccountingSettingDto>> UpdateAsync(int id, CreateAccountingSettingDto entity)
        {
            if (entity.Id != id)
                return BadRequest("Id mismatch");
            var result = await _service.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
