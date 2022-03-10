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
    public class WarehouseController : ControllerBase
    {
        private readonly IWarehouseService _warehouseService;

        public WarehouseController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<WarehouseDto>>>> GetAllAsync([FromQuery] PaginationFilter filter)
        {
            var warehouses = await _warehouseService.GetAllAsync(filter);
            if (warehouses.IsSuccess)
                return Ok(warehouses); // Status Code : 200

            return BadRequest(warehouses); // Status code : 400
        }
        [HttpPost]
        public async Task<ActionResult<Response<WarehouseDto>>> CreateAsync(CreateWarehouseDto entity)
        {
            var warehouse = await _warehouseService.CreateAsync(entity);
            if (warehouse.IsSuccess)
                return Ok(warehouse); // Status Code : 200

            return BadRequest(warehouse); // Status code : 400
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<WarehouseDto>>> GetByIdAsync(int id)
        {
            var result = await _warehouseService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<WarehouseDto>>> UpdateAsync(int id, CreateWarehouseDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _warehouseService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }
    }
}
