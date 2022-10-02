using Application.Contracts.DTOs;
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
    public class COAController : ControllerBase
    {
        private readonly ICOAService _coaService;

        public COAController(ICOAService coaService)
        {
            _coaService = coaService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.ChartOfAccountClaims.View, Permissions.Level4Claims.Create, Permissions.Level4Claims.View, Permissions.Level4Claims.Delete, Permissions.Level4Claims.Edit })]
        [HttpGet]
        public async Task<ActionResult<Response<List<Level1Dto>>>> GetChartOfAccounts()
        {
            return Ok(await _coaService.GetCOA()); // Status Code : 200
        }
        [AllowAnonymous]
        [HttpGet("download")]
        public async Task<ActionResult> DownloadCOA()
        {
            try
            {
                var stream = await _coaService.Export();
                string excelName = $"ChartOfAccount-{DateTime.Now:yyyyMMddHHmmssfff}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);

            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                     e.Message);
            }
        }

    }
}
