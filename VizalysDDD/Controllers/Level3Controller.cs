using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class Level3Controller : ControllerBase
    {
        private readonly ILevel3Service _level3Service;

        public Level3Controller(ILevel3Service level3Service)
        {
            _level3Service = level3Service;
        }

        [HttpGet("Dropdown")]
        public async Task<ActionResult<Response<List<Level3>>>> GetProductDropDown()
        {
            return Ok(await _level3Service.GetLevel3DropDown()); // Status Code : 200
        }
    }
}
