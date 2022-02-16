using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly IWarehouseService _warehouseService;

        public WarehouseController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        [HttpGet]
        public async Task<ActionResult<List<WarehouseDto>>> GetAllAsync([FromQuery] PaginationFilter filter)
        {
            var warehouse = await _warehouseService.GetAllAsync(filter);
            return Ok(warehouse);
        }
        [HttpPost]
        public async Task<ActionResult<WarehouseDto>> CreateAsync(CreateWarehouseDto entity)
        {
            var warehouse = await _warehouseService.CreateAsync(entity);
            return Ok(warehouse);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<WarehouseDto>> GetByIdAsync(int id)
        {
            var result = await _warehouseService.GetByIdAsync(id);
            return Ok(result); // Status Code : 200
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<WarehouseDto>> UpdateAsync(int id, CreateWarehouseDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _warehouseService.UpdateAsync(entity);
            return Ok(result); // Status Code : 200
        }
    }
}
