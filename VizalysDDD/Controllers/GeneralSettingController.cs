using Application.Contracts.DTOs.GeneralSetting;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralSettingController : ControllerBase
    {
        private readonly IGeneralSettingService _service;

        public GeneralSettingController(IGeneralSettingService service)
        {
            _service = service;
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<GeneralSettingDto>>> GetbyId(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if(result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
        [HttpPost]
        public async Task<ActionResult<Response<GeneralSettingDto>>> CreateAsync(CreateGeneralSettingDto entity)
        {
            var result = await _service.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
