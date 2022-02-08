using Application.Contracts.DTOs;
using Application.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
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

        [HttpGet]
        public async Task<ActionResult<List<ClientDto>>> GetAll()
        {
            var clients = await _clientService.GetAllAsync();
            return Ok(clients);
        }

        [HttpPost]
        public async Task<ActionResult<ClientDto>> Create(CreateClientDto entity)
        {
            var clients = await _clientService.CreateAsync(entity);
            return Ok(clients);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ClientDto>> Create(int id)
        {
            var result = await _clientService.GetByIdAsync(id);
            return Ok(result); // Status Code : 200
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ClientDto>> UpdateAsync(int id, CreateClientDto entity)
        {
            if (id != entity.Id)
                return BadRequest("Product ID mismatch");
            
            var result = await _clientService.UpdateAsync(entity);
            return Ok(result); // Status Code : 200
        }

        [HttpGet("Find")]
        public IActionResult Find()
        {
            var result = _clientService.Find();
            return Ok(result); // Status Code : 200
        }
    }
}
