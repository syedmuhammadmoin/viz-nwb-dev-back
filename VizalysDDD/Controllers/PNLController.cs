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
    public class PNLController : ControllerBase
    {
        private readonly IPNLReportService _pnlReportService;

        public PNLController(IPNLReportService pnlReportService)
        {
            _pnlReportService = pnlReportService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.ProfitLossClaims.View })]
        [HttpPost]
        public ActionResult<Response<List<PNLDto>>> GetProfitLoss(PNLFilters filters)
        {
            var pnl = _pnlReportService.GetProfitLoss(filters);
            if (pnl.IsSuccess)
                return Ok(pnl); // Status Code : 200

            return BadRequest(pnl); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.DashboardProfitLossClaims.View })]
        [HttpPost("SummaryforLast12Month")]
        public ActionResult<Response<List<PNLSummaryDTO>>> GetProfitLossSummaryforLast12Month()
        {
            var pnl = _pnlReportService.GetProfitLossSummaryforLast12Month();
            if (pnl.IsSuccess)
                return Ok(pnl); // Status Code : 200

            return BadRequest(pnl); // Status code : 400
        }


        
    }
}
