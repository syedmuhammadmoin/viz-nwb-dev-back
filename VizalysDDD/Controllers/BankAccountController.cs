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
    public class BankAccountController : ControllerBase
    {
        private readonly IBankAccountService _bankAccountService;

        public BankAccountController(IBankAccountService bankAccountService)
        {
            _bankAccountService = bankAccountService;
        }
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<BankAccountDto>>>> GetAllAsync([FromQuery] PaginationFilter filter)
        {
            return Ok(await _bankAccountService.GetAllAsync(filter)); // Status Code : 200
        }

        [HttpPost]
        public async Task<ActionResult<Response<BankAccountDto>>> CreateAsync(CreateBankAccountDto entity)
        {
            var bankAccount = await _bankAccountService.CreateAsync(entity);
            if (bankAccount.IsSuccess)
                return Ok(bankAccount); // Status Code : 200

            return BadRequest(bankAccount); // Status code : 400
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<BankAccountDto>>> GetByIdAsync(int id)
        {
            var result = await _bankAccountService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<BankAccountDto>>> UpdateAsync(int id, UpdateBankAccountDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _bankAccountService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }
    }
}
