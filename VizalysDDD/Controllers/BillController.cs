using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly IBillService _billService;

        public BillController(IBillService billService)
        {
            _billService = billService;
        }

        [HttpPost]
        public async Task<ActionResult<Response<BillDto>>> CreateAsync(CreateBillDto entity)
        {
            var bill = await _billService.CreateAsync(entity);
            if (bill.IsSuccess)
                return Ok(bill); // Status Code : 200

            return BadRequest(bill); // Status code : 400
        }

        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<BillDto>>>> GetAllAsync([FromQuery] PaginationFilter filter)
        {
            var bills = await _billService.GetAllAsync(filter);
            if (bills.IsSuccess)
                return Ok(bills); // Status Code : 200

            return BadRequest(bills); // Status code : 400
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<BillDto>>> GetByIdAsync(int id)
        {
            var result = await _billService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

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
    }
}
