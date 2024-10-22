using Application.Contracts.DTOs.FiscalPeriod;
using Application.Contracts.DTOs.FiscalPeriodSetting;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FiscalPeriodSettingController : ControllerBase
    {
        private readonly IFiscalPeriodSettingService _service;

        public FiscalPeriodSettingController(IFiscalPeriodSettingService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<List<FiscalPeriodSettingDto>>> GetAllAsync([FromQuery] TransactionFormFilter? filter)
        {
            var result = await _service.GetAllAsync(filter);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<FiscalPeriodSettingDto>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost]
        public async Task<ActionResult<FiscalPeriodSettingDto>> CreateAsync(CreateFiscalPeriodSettingDto entity)
        {
            var result = await _service.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<FiscalPeriodSettingDto>> UpdateAsync(int id, CreateFiscalPeriodSettingDto entity)
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
