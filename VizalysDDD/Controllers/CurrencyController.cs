using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Helper;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CurrencyController  : ControllerBase
{
    private readonly ICurrencyService _CurrencyService;

    public CurrencyController(ICurrencyService CurrencyService)
    {
        _CurrencyService = CurrencyService;
    }

    [ClaimRequirement("Permission", new string[] { Permissions.CurrencyClaims.Create, Permissions.CurrencyClaims.View, Permissions.CurrencyClaims.Delete, Permissions.CurrencyClaims.Edit })]
    [HttpGet]
    public async Task<ActionResult<PaginationResponse<List<CurrencyDto>>>> GetAllAsync([FromQuery] CurrencyFilter filter)
    {
        var results = await _CurrencyService.GetAllAsync(filter);
        if (results.IsSuccess)
            return Ok(results); // Status Code : 200

        return BadRequest(results); // Status code : 400
    }

    [ClaimRequirement("Permission", new string[] { Permissions.CurrencyClaims.Create, Permissions.CurrencyClaims.View, Permissions.CurrencyClaims.Delete, Permissions.CurrencyClaims.Edit })]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CurrencyDto>> GetByIdAsync(int id)
    {
        var result = await _CurrencyService.GetByIdAsync(id);
        if (result.IsSuccess)
            return Ok(result); // Status Code : 200

        return BadRequest(result); // Status code : 400
    }

    [HttpGet("Dropdown")]
    public async Task<ActionResult<Response<List<CurrencyDto>>>> GetDropDown()
    {

        var result = await _CurrencyService.GetDropDown();
        if (result.IsSuccess)
            return Ok(result); // Status Code : 200

        return BadRequest(result); // Status code : 400
    }

    [ClaimRequirement("Permission", new string[] { Permissions.CurrencyClaims.Create })]
    [HttpPost]
    public async Task<ActionResult<Response<CurrencyDto>>> CreateAsync(CreateCurrencyDto entity)
    {
        var result = await _CurrencyService.CreateAsync(entity);
        if (result.IsSuccess)
            return Ok(result); // Status Code : 200

        return BadRequest(result); // Status code : 400
    }

    [ClaimRequirement("Permission", new string[] { Permissions.CurrencyClaims.Edit })]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<Response<CurrencyDto>>> UpdateAsync(int id, CreateCurrencyDto entity)
    {
        if (id != entity.Id)
            return BadRequest("Id mismatch");

        var result = await _CurrencyService.UpdateAsync(entity);
        if (result.IsSuccess)
            return Ok(result); // Status Code : 200

        return BadRequest(result); // Status code : 400
    }

   
}
