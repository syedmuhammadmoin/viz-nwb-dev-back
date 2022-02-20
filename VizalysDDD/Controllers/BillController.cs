using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
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
        public async Task<ActionResult<BillDto>> CreateAsync(CreateBillDto entity)
        {
            var bill = await _billService.CreateAsync(entity);
            return Ok(bill);
        }

        [HttpGet]
        public async Task<ActionResult<List<BillDto>>> GetAllAsync([FromQuery] PaginationFilter filter)
        {
            var bills = await _billService.GetAllAsync(filter);
            return Ok(bills);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BillDto>> GetByIdAsync(int id)
        {
            var result = await _billService.GetByIdAsync(id);
            return Ok(result); // Status Code : 200
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<BillDto>> UpdateAsync(int id, CreateBillDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _billService.UpdateAsync(entity);
            return Ok(result); // Status Code : 200
        }
    }
}
