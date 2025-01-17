﻿using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Helper;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DebitNoteController : ControllerBase
    {
        private readonly IDebitNoteService _debitNoteService;
        private readonly IFileuploadServices _fileuploadServices;
        public DebitNoteController(IDebitNoteService debitNoteService, IFileuploadServices fileuploadServices)
        {
            _debitNoteService = debitNoteService;
            _fileuploadServices = fileuploadServices;

        }

        [ClaimRequirement("Permission", new string[] { Permissions.DebitNoteClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<DebitNoteDto>>> CreateAsync(CreateDebitNoteDto entity)
        {
            var result = await _debitNoteService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.DebitNoteClaims.Create, Permissions.DebitNoteClaims.View, Permissions.DebitNoteClaims.Delete, Permissions.DebitNoteClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<DebitNoteDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var results = await _debitNoteService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.DebitNoteClaims.Create, Permissions.DebitNoteClaims.View, Permissions.DebitNoteClaims.Delete, Permissions.DebitNoteClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<DebitNoteDto>>> GetByIdAsync(int id)
        {
            var result = await _debitNoteService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.DebitNoteClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<DebitNoteDto>>> UpdateAsync(int id, CreateDebitNoteDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _debitNoteService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpPost("workflow")]
        public async Task<ActionResult<Response<bool>>> CheckWorkFlow([FromBody] ApprovalDto data)
        {
            var result = await _debitNoteService.CheckWorkFlow(data);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200
            return BadRequest(result);
        }
        [HttpPost("DocUpload/{id:int}")]
        public async Task<ActionResult<bool>> UploadFile(IFormFile file, int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _fileuploadServices.UploadFile(file, id, DocType.DebitNote);
                    if (result.IsSuccess)
                        return Ok(result); // Status Code : 200
                    return BadRequest(result);
                }
                return BadRequest("Some properties are not valid"); // Status code : 400
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.Message);
            }
        }
    }
}
