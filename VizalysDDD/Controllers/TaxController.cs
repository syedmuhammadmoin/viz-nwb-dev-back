using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Helper;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Application.Services;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaxController : ControllerBase
    {
        private readonly ITaxService _taxService;

        public TaxController(ITaxService taxService)
        {
            _taxService = taxService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.TaxesClaims.Create, Permissions.TaxesClaims.View, Permissions.TaxesClaims.Delete, Permissions.TaxesClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<TaxDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var results = await _taxService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.TaxesClaims.Create, Permissions.TaxesClaims.View, Permissions.TaxesClaims.Delete, Permissions.TaxesClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<TaxDto>> GetByIdAsync(int id)
        {
            var result = await _taxService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }
        [ClaimRequirement("Permission", new string[] { Permissions.TaxesClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<CategoryDto>>> CreateAsync(CreateTaxDto entity)
        {
            var result = await _taxService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.TaxesClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<TaxDto>>> UpdateAsync(int id, CreateTaxDto entity)
        {
            if (id != entity.Id)
                return BadRequest("Id mismatch");

            var result = await _taxService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }
        [HttpDelete]
        public async Task<ActionResult<Response<string>>> DeleteTaxes(List<int> ids)
        {
            var result = await _taxService.DeleteTaxes(ids);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
        }
    }
}
