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
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<LocationDto>>>> GetAllAsync([FromQuery] PaginationFilter filter)
        {
            var location = await _locationService.GetAllAsync(filter);
            if (location.IsSuccess)
                return Ok(location); // Status Code : 200

            return BadRequest(location); // Status code : 400
        }
        [HttpPost]
        public async Task<ActionResult<Response<LocationDto>>> CreateAsync(CreateLocationDto entity)
        {
            var location = await _locationService.CreateAsync(entity);
            if (location.IsSuccess)
                return Ok(location); // Status Code : 200

            return BadRequest(location); // Status code : 400
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<LocationDto>>> GetByIdAsync(int id)
        {
            var result = await _locationService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<LocationDto>>> UpdateAsync(int id, CreateLocationDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _locationService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }
    }

}
