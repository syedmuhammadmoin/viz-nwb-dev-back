using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
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
        public async Task<ActionResult<List<LocationDto>>> GetAllAsync([FromQuery] PaginationFilter filter)
        {
            var location = await _locationService.GetAllAsync(filter);
            return Ok(location);
        }
        [HttpPost]
        public async Task<ActionResult<LocationDto>> CreateAsync(CreateLocationDto entity)
        {
            var location = await _locationService.CreateAsync(entity);
            return Ok(location);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<LocationDto>> GetByIdAsync(int id)
        {
            var result = await _locationService.GetByIdAsync(id);
            return Ok(result); // Status Code : 200
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<LocationDto>> UpdateAsync(int id, CreateLocationDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _locationService.UpdateAsync(entity);
            return Ok(result); // Status Code : 200
        }
    }

}
