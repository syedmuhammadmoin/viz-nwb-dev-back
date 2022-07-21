using Application.Contracts.DTOs;
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

    public class BillController : ControllerBase
    {
        private readonly IBillService _billService;
        private readonly IFileuploadServices _fileUploadService;

        public BillController(IBillService billService, IFileuploadServices fileUploadService)
        {
            _billService = billService;
            _fileUploadService = fileUploadService;


        }

        [ClaimRequirement("Permission", new string[] { Permissions.BillClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<BillDto>>> CreateAsync(CreateBillDto entity)
        {
            var bill = await _billService.CreateAsync(entity);
            if (bill.IsSuccess)
                return Ok(bill); // Status Code : 200

            return BadRequest(bill); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.BillClaims.Create, Permissions.BillClaims.View, Permissions.BillClaims.Delete, Permissions.BillClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<BillDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var bills = await _billService.GetAllAsync(filter);
            if (bills.IsSuccess)
                return Ok(bills); // Status Code : 200

            return BadRequest(bills); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.BillClaims.Create, Permissions.BillClaims.View, Permissions.BillClaims.Delete, Permissions.BillClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<BillDto>>> GetByIdAsync(int id)
        {
            var result = await _billService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.BillClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<BillDto>>> UpdateAsync(int id, CreateBillDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _billService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpPost("workflow")]
        public async Task<ActionResult<Response<bool>>> CheckWorkFlow([FromBody] ApprovalDto data)
        {
            var result = await _billService.CheckWorkFlow(data);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200
            return BadRequest(result);
        }

        [HttpPost("DocUpload/{id:int}")]
        public async Task<ActionResult<Response<int>>> UploadFile(IFormFile file, int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _fileUploadService.UploadFile(file, id, DocType.Bill);
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
