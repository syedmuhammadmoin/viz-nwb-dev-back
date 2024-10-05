using Application.Contracts.DTOs.TaxGroup;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxGroupController : ControllerBase
    {
        private readonly ITaxGroupService _service;

        public TaxGroupController(ITaxGroupService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<List<TaxGroupDto>>> GetAllAsync([FromQuery] TransactionFormFilter? filter)
        {
            var result = await _service.GetAllAsync(filter);
            if(result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<TaxGroupDto>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost]
        public async Task<ActionResult<TaxGroupDto>> CreateAsync(CreateTaxGroupDto entity)
        {
            var result = await _service.CreateAsync(entity);
            if(result.IsSuccess)
                return Ok(result);  
            return BadRequest(result);
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult<TaxGroupDto>> UpdateAsync(int id, CreateTaxGroupDto entity)
        {
            if(entity.Id != id)
                return BadRequest("Id mismatch");
            var result = await _service.UpdateAsync(entity);
            if(result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpDelete]
        public async Task<ActionResult<Response<string>>> DeleteTaxes(List<int> ids)
        {
            var result = await _service.DeleteTaxGroups(ids);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
