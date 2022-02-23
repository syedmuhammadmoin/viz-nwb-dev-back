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
    public class Level4Controller : ControllerBase
    {
        private readonly ILevel4Service _level4Service;

        public Level4Controller(ILevel4Service level4Service)
        {
            _level4Service = level4Service;
        }

        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<Level4Dto>>>> GetAllAsync([FromQuery] PaginationFilter filter)
        {
            var level4 = await _level4Service.GetAllAsync(filter);
            if (level4.IsSuccess)
                return Ok(level4); // Status Code : 200

            return BadRequest(level4); // Status code : 400
        }

        [HttpPost]
        public async Task<ActionResult<Response<Level4Dto>>> CreateAsync(CreateLevel4Dto entity)
        {
            var level4 = await _level4Service.CreateAsync(entity);
            if (level4.IsSuccess)
                return Ok(level4); // Status Code : 200

            return BadRequest(level4); // Status code : 400
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<Response<Level4Dto>>> GetByIdAsync(Guid id)
        {
            var result = await _level4Service.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpPut("{id:Guid}")]
        public async Task<ActionResult<Response<Level4Dto>>> UpdateAsync(Guid id, CreateLevel4Dto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _level4Service.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }
    }
}