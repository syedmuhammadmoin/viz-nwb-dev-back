using Application.Contracts.DTOs;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIController : Controller
    {
        private readonly ICityService _cityService;

        public APIController(ICityService cityService)
        {
            _cityService = cityService;
        }
        [HttpGet()]
        public async Task<ActionResult<Response<List<CityDto>>>> GetDropDown()
        {
            var result = await _cityService.GetDropDown();
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }
    }
}
