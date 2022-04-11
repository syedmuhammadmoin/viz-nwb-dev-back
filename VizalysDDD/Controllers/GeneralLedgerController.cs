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
    public class GeneralLedgerController : ControllerBase
    {
        private readonly IGeneralLedgerReportService _generalLedgerReportService;

        public GeneralLedgerController(IGeneralLedgerReportService generalLedgerReportService)
        {
            _generalLedgerReportService = generalLedgerReportService;
        }

        [ClaimRequirement("Permission", new string[] { Permissions.GeneralLedgerClaims.View })]
        [HttpPost]
        public ActionResult<Response<List<GeneralLedgerDto>>> GetGeneralLedgerRecords(GeneralLedgerFilters filters)
        {
            var generalLedger = _generalLedgerReportService.GetLedger(filters);
            if (generalLedger.IsSuccess)
                return Ok(generalLedger); // Status Code : 200

            return BadRequest(generalLedger); // Status code : 400
        }
    }
}
