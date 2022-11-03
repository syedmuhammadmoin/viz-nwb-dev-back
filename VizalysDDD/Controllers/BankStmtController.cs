using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Helper;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Data;

namespace Vizalys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BankStmtController : ControllerBase
    {
        private readonly IBankStmtService _bankStmtService;

        public BankStmtController(IBankStmtService bankStmtService)
        {
            _bankStmtService = bankStmtService;
        }
        
        [ClaimRequirement("Permission", new string[] { Permissions.BankStatementClaims.Create, Permissions.BankStatementClaims.View, Permissions.BankStatementClaims.Delete, Permissions.BankStatementClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<BankStmtDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            return Ok(await _bankStmtService.GetAllAsync(filter)); // Status Code : 200
        }
        
        [ClaimRequirement("Permission", new string[] { Permissions.BankStatementClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<BankStmtDto>>> CreateAsync([ModelBinder(BinderType = typeof(JsonModelBinder))] CreateBankStmtDto data, IFormFile? files)
        {
            var bankStmt = await _bankStmtService.CreateAsync(data, files);
            if (bankStmt.IsSuccess)
                return Ok(bankStmt); // Status Code : 200

            return BadRequest(bankStmt); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.BankStatementClaims.Create, Permissions.BankStatementClaims.View, Permissions.BankStatementClaims.Delete, Permissions.BankStatementClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<BankStmtDto>>> GetByIdAsync(int id)
        {
            var result = await _bankStmtService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.BankStatementClaims.Edit })]
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

        [Authorize]
        [ClaimRequirement("Permission", new string[] { Permissions.BankReconClaims.Edit, Permissions.BankReconClaims.Create, Permissions.BankReconClaims.View })]
        [HttpGet("bankStatus/{id:int}")]
        public ActionResult<Response<List<UnReconStmtDto>>> GetBankUnreconciledStmts(int id)
        {
            var result = _bankStmtService.GetBankUnreconciledStmts(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [AllowAnonymous]
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
            dt.Columns.Add("Reference (In Numbers)", typeof(int));
            dt.Columns.Add("StmtDate", typeof(DateTime));
            dt.Columns.Add("Label", typeof(string));
            dt.Columns.Add("Debit", typeof(decimal));
            dt.Columns.Add("Credit", typeof(decimal));
            dt.AcceptChanges();
            return dt;
        }
    }
}
