﻿using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
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
        public async Task<ActionResult<PaginationResponse<List<CategoryDto>>>> GetAllAsync([FromQuery] PaginationFilter filter)
        {
            var results = await _categoryService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }

        [HttpPost]
        public async Task<ActionResult<Response<CategoryDto>>> CreateAsync(CreateCategoryDto entity)
        {
            var result = await _categoryService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryDto>> GetByIdAsync(int id)
        {
            var result = await _categoryService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<CategoryDto>>> UpdateAsync(int id, CreateCategoryDto entity)
        {
            if (id != entity.Id)
                return BadRequest("Id mismatch");

            var result = await _categoryService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }
    }
}