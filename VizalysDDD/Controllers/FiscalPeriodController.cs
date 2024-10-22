using Application.Contracts.DTOs.FiscalPeriod;
using Application.Contracts.DTOs.TaxGroup;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FiscalPeriodController : ControllerBase
    {
        private readonly IFiscalPeriodService _service;

        public FiscalPeriodController(IFiscalPeriodService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<List<FiscalPeriodDto>>> GetAllAsync([FromQuery] TransactionFormFilter? filter)
        {
            var result = await _service.GetAllAsync(filter);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<FiscalPeriodDto>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost]
        public async Task<ActionResult<FiscalPeriodDto>> CreateAsync(CreateFiscalPeriodDto entity)
        {
            var result = await _service.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<FiscalPeriodDto>> UpdateAsync(int id, CreateFiscalPeriodDto entity)
        {
            if (entity.Id != id)
                return BadRequest("Id mismatch");
            var result = await _service.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpDelete]
        public async Task<ActionResult<Response<string>>> DeleteTaxes(List<int> ids)
        {
            var result = await _service.DeleteBulkRecords(ids);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
