using Application.Contracts.DTOs;
using Application.Contracts.Helper;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Domain.Constants;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }
        // /api/auth/login
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<Response<bool>>> LoginAsync([FromBody] LoginDto model)
        {
            var result = await _userService.LoginUserAsync(model);

            if (result.IsSuccess)
                return Ok(result);// Status Code : 200

            return BadRequest(result); // Status code : 400

        }

        //  /api/auth/RegisterUser
        [ClaimRequirement("Permission", new string[] { Permissions.AuthClaims.Create })]
        [HttpPost("Users")]
        public async Task<ActionResult<Response<bool>>> RegisterUserAsync([FromBody] RegisterUserDto model)
        {
            var result = await _userService.RegisterUserAsync(model);

            if (result.IsSuccess)
                return Ok(result); // Status Code : 200
            return BadRequest(result); // Status code : 400
        }

        //  /api/auth/GetUser
        [ClaimRequirement("Permission", new string[] { Permissions.AuthClaims.Create, Permissions.AuthClaims.View, Permissions.AuthClaims.Delete, Permissions.AuthClaims.Edit })]
        [HttpGet("Users")]
        public async Task<ActionResult<Response<IEnumerable<User>>>> GetUsersAsync()
        {
            var result = await _userService.GetUsersAsync();

            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        //  /api/auth/GetUserByID
        [ClaimRequirement("Permission", new string[] { Permissions.AuthClaims.View, Permissions.AuthClaims.Delete, Permissions.AuthClaims.Edit })]
        [HttpGet("Users/{id:Guid}")]
        public async Task<ActionResult<Response<EditUserDto>>> GetUserAsync(string id)
        {

            var result = await _userService.GetUserAsync(id);

            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400

        }

        // /api/auth/UpdateUser
        [ClaimRequirement("Permission", new string[] { Permissions.AuthClaims.Edit })]
        [HttpPut("Users/{id:Guid}")]
        public async Task<ActionResult<Response<bool>>> UpdateUserAsync(string id, EditUserDto model)
        {
            var result = await _userService.UpdateUserAsync(id, model);

            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        // /api/auth/User/ResetPass/id
        [ClaimRequirement("Permission", new string[] { Permissions.AuthClaims.Edit })]
        [HttpPut("Users/ResetPass/{id:Guid}")]
        public async Task<ActionResult<Response<bool>>> ResetUserPassword(string id, ResetPasswordDto model)
        {
            var result = await _userService.ResetUserPassword(id, model);

            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result);// Status code : 400
        }

        // /api/auth/User/changePassword/id
        [HttpPut("Users/changePassword/{id:Guid}")]
        public async Task<ActionResult<Response<bool>>> ChangePassword(string id, ChangePasswordDto model)
        {
            var result = await _userService.ChangePassword(id, model);

            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        // For Roles
        [ClaimRequirement("Permission", new string[] { Permissions.AuthClaims.Create })]
        [HttpPost("Roles")]
        public async Task<ActionResult<Response<string>>> CreateRoleAsync([FromBody] RegisterRoleDto model)
        {
            var result = await _userService.CreateRoleAsync(model);
            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result);// Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.AuthClaims.Create, Permissions.AuthClaims.View, Permissions.AuthClaims.Delete, Permissions.AuthClaims.Edit })]
        [HttpGet("Roles")]
        public async Task<ActionResult<Response<IEnumerable<IdentityRole>>>> GetRolesAsync()
        {
            var result = await _userService.GetRolesAsync();

            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        [ClaimRequirement("Permission", new string[] { Permissions.AuthClaims.View, Permissions.AuthClaims.Delete, Permissions.AuthClaims.Edit })]
        [HttpGet("Roles/{id:Guid}")]
        public async Task<ActionResult<Response<RegisterRoleDto>>> GetRolesAsync(string id)
        {
            var result = await _userService.GetRoleAsync(id);

            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }
        
        [ClaimRequirement("Permission", new string[] { Permissions.AuthClaims.Edit })]
        [HttpPut("Roles/{id:Guid}")]
        public async Task<ActionResult<Response<RegisterRoleDto>>> UpdateRoleAsync(string id, RegisterRoleDto model)
        {
            var result = await _userService.UpdateRoleAsync(id, model);

            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        // /api/auth/Claims
        [HttpGet("Claims")]
        public ActionResult<Response<List<string>>> GetClaims()
        {
            var result = _userService.GetClaimsAsync();

            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result);// Status code : 400
        }
    }
}