using Application.Contracts.DTOs;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionReconController : ControllerBase
    {
        private readonly ITransactionReconcileService _transactionReconcileService;

        public TransactionReconController(ITransactionReconcileService transactionReconcileService)
        {
            _transactionReconcileService = transactionReconcileService;
        }

        [HttpPost]
        public async Task<ActionResult<Response<bool>>> Reconcile(CreateTransactionReconcileDto data)
        {
            try
            {
                    var result = await _transactionReconcileService.Reconcile(data);
                    if (result.IsSuccess)
                        return Ok(result); // Status Code : 200
                    return BadRequest(result);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.Message);
            }
        }
    }
}
