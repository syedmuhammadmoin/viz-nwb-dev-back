using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpPost]
        public async Task<ActionResult<Response<ClientDto>>> CreateAsync(CreateClientDto entity)
        {
            var result = await _clientService.CreateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<ClientDto>>>> GetAllAsync([FromQuery] PaginationFilter filter)
        {
            var results = await _clientService.GetAllAsync(filter);
            if (results.IsSuccess)
                return Ok(results); // Status Code : 200

            return BadRequest(results); // Status code : 400
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<ClientDto>>> GetByIdAsync(int id)
        {
            var result = await _clientService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<ClientDto>>> UpdateAsync(int id, CreateClientDto entity)
        {
            if (id != entity.Id)
                return BadRequest("Id mismatch");
            
            var result = await _clientService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

    }
}
