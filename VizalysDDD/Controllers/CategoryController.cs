using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryDto>>> GetAllAsync([FromQuery] PaginationFilter filter)
        {
            var categorys = await _categoryService.GetAllAsync(filter);
            return Ok(categorys);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDto>> CreateAsync(CreateCategoryDto entity)
        {
            var categorys = await _categoryService.CreateAsync(entity);
            return Ok(categorys);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryDto>> GetByIdAsync(int id)
        {
            var result = await _categoryService.GetByIdAsync(id);
            return Ok(result); // Status Code : 200
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CategoryDto>> UpdateAsync(int id, CreateCategoryDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _categoryService.UpdateAsync(entity);
            return Ok(result); // Status Code : 200
        }
    }
}
