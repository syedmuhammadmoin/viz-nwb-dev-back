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
    public class TrialBalanceController : ControllerBase
    {
        private readonly ITrialBalanceReportService _trialBalanceReportService;

        public TrialBalanceController(ITrialBalanceReportService trialBalanceReportService)
        {
            _trialBalanceReportService = trialBalanceReportService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.TrialBalanceClaims.View })]
        [HttpPost]
        public ActionResult<Response<List<TrialBalanceDto>>> GetTrialBalance(TrialBalanceFilters filters)
        {
            var trialBalance = _trialBalanceReportService.GetTrialBalance(filters);
            if (trialBalance.IsSuccess)
                return Ok(trialBalance); // Status Code : 200

            return BadRequest(trialBalance); // Status code : 400
        }

    }
}
