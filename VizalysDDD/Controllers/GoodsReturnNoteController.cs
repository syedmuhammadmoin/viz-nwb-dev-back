﻿using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Helper;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Microsoft.AspNetCore.Authorization;
using Domain.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace Vizalys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GoodsReturnNoteController : ControllerBase
    {
        private readonly IGoodsReturnNoteService _goodsReturnNoteService;

        public GoodsReturnNoteController(IGoodsReturnNoteService goodsReturnNoteService)
        {
            _goodsReturnNoteService = goodsReturnNoteService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.GoodsReturnNoteClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<GoodsReturnNoteDto>>> CreateAsync(CreateGoodsReturnNoteDto entity)
        {
            var result = await _goodsReturnNoteService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.GoodsReturnNoteClaims.Create, Permissions.GoodsReturnNoteClaims.View, Permissions.GoodsReturnNoteClaims.Delete, Permissions.GoodsReturnNoteClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<GoodsReturnNoteDto>>>> GetAllAsync([FromQuery] PaginationFilter filter)
        {
            var results = await _goodsReturnNoteService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.GoodsReturnNoteClaims.Create, Permissions.GoodsReturnNoteClaims.View, Permissions.GoodsReturnNoteClaims.Delete, Permissions.GoodsReturnNoteClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<GoodsReturnNoteDto>>> GetByIdAsync(int id)
        {
            var result = await _goodsReturnNoteService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.GoodsReturnNoteClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<GoodsReturnNoteDto>>> UpdateAsync(int id, CreateGoodsReturnNoteDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _goodsReturnNoteService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpPost("workflow")]
        public async Task<ActionResult<Response<bool>>> CheckWorkFlow([FromBody] ApprovalDto data)
        {
            var result = await _goodsReturnNoteService.CheckWorkFlow(data);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200
            return BadRequest(result);
        }
    }
}
