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

    public class BalanceSheetController : ControllerBase
    {
        private readonly IBalanceSheetReportService _balanceSheetReportService;

        public BalanceSheetController(IBalanceSheetReportService balanceSheetReportService)
        {
            _balanceSheetReportService = balanceSheetReportService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.BalanceSheetClaims.View })]
        [HttpPost]
        public ActionResult<Response<List<BalanceSheetDto>>> GetBalanceSheetRecords(BalanceSheetFilters filters)
        {
            var balanceSheet = _balanceSheetReportService.GetBalanceSheet(filters);
            if (balanceSheet.IsSuccess)
                return Ok(balanceSheet); // Status Code : 200

            return BadRequest(balanceSheet); // Status code : 400
        }
    }
}
