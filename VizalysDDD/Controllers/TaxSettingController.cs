using Application.Contracts.DTOs.TaxSetting;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxSettingController : ControllerBase
    {
        private readonly ITaxSettingService _service;

        public TaxSettingController(ITaxSettingService service)
        {
            _service = service;
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<TaxSettingDto>>> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if(result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost]
        public async Task<ActionResult<Response<TaxSettingDto>>> CreateAsync(CreateTaxSettingDto entity)
        {
            var result = await _service.CreateAsync(entity);
            if(result.IsSuccess) 
                return Ok(result);
            return BadRequest(result);
        }
    }
}
