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
    public class PettyCashController : ControllerBase
    {
        private readonly IPettyCashService _pettyCashService;
        private readonly IFileuploadServices _fileUploadService;
        public PettyCashController(IPettyCashService pettyCashService, IFileuploadServices fileUploadService)
        {
            _pettyCashService = pettyCashService;
            _fileUploadService = fileUploadService;

        }

        [ClaimRequirement("Permission", new string[] { Permissions.PettyCashClaims.Create })]
        [HttpPost]
        public async Task<ActionResult<Response<PettyCashDto>>> CreateAsync(CreatePettyCashDto entity)
        {
            var pettyCash = await _pettyCashService.CreateAsync(entity);
            if (pettyCash.IsSuccess)
                return Ok(pettyCash); // Status Code : 200

            return BadRequest(pettyCash); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.PettyCashClaims.Create, Permissions.PettyCashClaims.View, Permissions.PettyCashClaims.Delete, Permissions.PettyCashClaims.Edit })]
        [HttpGet]
        public async Task<ActionResult<PaginationResponse<List<JournalEntryDto>>>> GetAllAsync([FromQuery] TransactionFormFilter filter)
        {
            var pettyCashList = await _pettyCashService.GetAllAsync(filter);
            if (pettyCashList.IsSuccess)
                return Ok(pettyCashList); // Status Code : 200

            return BadRequest(pettyCashList); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.PettyCashClaims.Create, Permissions.PettyCashClaims.View, Permissions.PettyCashClaims.Delete, Permissions.PettyCashClaims.Edit })]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Response<PettyCashDto>>> GetByIdAsync(int id)
        {
            var result = await _pettyCashService.GetByIdAsync(id);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.PettyCashClaims.Edit })]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<Response<PettyCashDto>>> UpdateAsync(int id, CreatePettyCashDto entity)
        {
            if (id != entity.Id)
                return BadRequest("ID mismatch");

            var result = await _pettyCashService.UpdateAsync(entity);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [HttpPost("workflow")]
        public async Task<ActionResult<Response<bool>>> CheckWorkFlow([FromBody] ApprovalDto data)
        {
            var result = await _pettyCashService.CheckWorkFlow(data);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200
            return BadRequest(result);

        }

        [HttpPost("DocUpload/{id:int}")]
        public async Task<ActionResult<Response<int>>> UploadFile(IFormFile file, int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _fileUploadService.UploadFile(file, id, DocType.JournalEntry);
                    if (result.IsSuccess)
                        return Ok(result); // Status Code : 200
                    return BadRequest(result);
                }
                return BadRequest("Some properties are not valid"); // Status code : 400
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.Message);
            }
        }
    }
}
