﻿using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Helper;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Data;

namespace Vizalys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankStmtController : ControllerBase
    {
        private readonly IBankStmtService _bankStmtService;

        public BankStmtController(IBankStmtService bankStmtService)
        {
            _bankStmtService = bankStmtService;
        }
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<BankStmtDto>>>> GetAllAsync([FromQuery] PaginationFilter filter)
        {
            return Ok(await _bankStmtService.GetAllAsync(filter)); // Status Code : 200
        }

        [HttpPost]
        public async Task<ActionResult<Response<BankStmtDto>>> CreateAsync([ModelBinder(BinderType = typeof(JsonModelBinder))] CreateBankStmtDto entity, IFormFile files)
        {
            var bankStmt = await _bankStmtService.CreateAsync(entity, files);
            if (bankStmt.IsSuccess)
                return Ok(bankStmt); // Status Code : 200

            return BadRequest(bankStmt); // Status code : 400
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<BankStmtDto>>> GetByIdAsync(int id)
        {
            var result = await _bankStmtService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<BankStmtDto>>> UpdateAsync(int id, CreateBankStmtDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _bankStmtService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpGet("getStmtFile")]
        public ActionResult ExportStmtFormat()
        {
            try
            {
                DataTable dt = getData();
                var stream = new MemoryStream();
                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromDataTable(dt, true);
                    package.Save();
                }
                stream.Position = 0;
                string excelName = $"BankStmt-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.Message);
            }
        }

        private DataTable getData()
        {
            //Creating DataTable  
            DataTable dt = new DataTable();
            //Setiing Table Name  
            dt.TableName = "BankStmtLines";
            //Add Columns  
            dt.Columns.Add("Reference", typeof(int));
            dt.Columns.Add("StmtDate", typeof(DateTime));
            dt.Columns.Add("Label", typeof(string));
            dt.Columns.Add("Debit", typeof(float));
            dt.Columns.Add("Credit", typeof(float));
            dt.AcceptChanges();
            return dt;
        }
    }
}