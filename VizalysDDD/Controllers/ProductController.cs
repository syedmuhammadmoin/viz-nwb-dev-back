using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Helper;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.ProductsClaims.Create, Permissions.ProductsClaims.View, Permissions.ProductsClaims.Delete, Permissions.ProductsClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<ProductDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var products = await _productService.GetAllAsync(filter);
            if (products.IsSuccess)
                return Ok(products); // Status Code : 200

            return BadRequest(products); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.ProductsClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<ProductDto>>> CreateAsync(CreateProductDto entity)
        {
            var product = await _productService.CreateAsync(entity);
            if (product.IsSuccess)
                return Ok(product); // Status Code : 200

            return BadRequest(product); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.ProductsClaims.View, Permissions.ProductsClaims.Delete, Permissions.ProductsClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<ProductDto>>> GetByIdAsync(int id)
        {
            var result = await _productService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.ProductsClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<ProductDto>>> UpdateAsync(int id, CreateProductDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _productService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpGet("Dropdown")]
        public async Task<ActionResult<Response<List<ProductDto>>>> GetProductDropDown()
        {
            return Ok(await _productService.GetProductDropDown()); // Status Code : 200
        }

    }
}
