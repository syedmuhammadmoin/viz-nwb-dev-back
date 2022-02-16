using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> GetAllAsync([FromQuery] PaginationFilter filter)
        {
            var product = await _productService.GetAllAsync(filter);
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateAsync(CreateProductDto entity)
        {
            var product = await _productService.CreateAsync(entity);
            return Ok(product);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetByIdAsync(int id)
        {
            var result = await _productService.GetByIdAsync(id);
            return Ok(result); // Status Code : 200
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProductDto>> UpdateAsync(int id, CreateProductDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _productService.UpdateAsync(entity);
            return Ok(result); // Status Code : 200
        }

    }
}
