using Application.Contracts.DTOs;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class COAController : ControllerBase
    {
        private readonly ICOAService _coaService;

        public COAController(ICOAService coaService)
        {
            _coaService = coaService;
        }

        [HttpGet]
        public async Task<ActionResult<Response<List<Level1Dto>>>> GetChartOfAccounts()
        {
            return Ok(await _coaService.GetCOA()); // Status Code : 200
        }

    }
}
