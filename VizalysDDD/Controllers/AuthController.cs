﻿using Application.Contracts.DTOs;
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
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public AuthController(IUserService userService, UserManager<User> userManager, IConfiguration configuration)
        {
            _userService = userService;
            _userManager = userManager;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("LoginSAAS")]
        public async Task<ActionResult<Response<bool>>> LoginUserSAAS([FromBody] LoginSAASDto model)
        {
            var result = await _userService.LoginUserSAAS(model);
            if (result.IsSuccess)
            {
                SetTokenInCookie("token", result.Result.SAASToken, DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["AuthSettings:SaaSJWTDurationInMinutes"])));
                return Ok(result);// Status Code : 200
            }

            return BadRequest(result); // Status code : 400

        }

        // /api/auth/login
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<Response<bool>>> LoginAsync([FromBody] LoginAPPDto model)
        {
            var result = await _userService.LoginUserAsync(model);

            if (result.IsSuccess)
                return Ok(result);// Status Code : 200

            return BadRequest(result); // Status code : 400

        }
        [HttpPost("ChangeOrganization")]
        public async Task<ActionResult<Response<AuthenticationDto>>> ChangeOrganization([FromBody] int orgId)
        {
            var result = await _userService.ChangeOrganization(orgId);

            if (result.IsSuccess)
            {
                SetTokenInCookie("refresh-token", result.Result.RefreshToken, DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["AuthSettings:RefreshTokenDurationInMinutes"])));
                return Ok(result);// Status Code : 200
            }

            return BadRequest(result); // Status code : 400

        }
        //  /api/auth/RegisterUser

        [AllowAnonymous]
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
        public async Task<ActionResult<Response<IEnumerable<UsersListDto>>>> GetUsersAsync()
        {
            var result = await _userService.GetUsersAsync();

            if (result.IsSuccess)
                return Ok(result); // Status Code : 200

            return BadRequest(result); // Status code : 400
        }

        //  /api/auth/GetUserByID
        [ClaimRequirement("Permission", new string[] { Permissions.AuthClaims.Create, Permissions.AuthClaims.View, Permissions.AuthClaims.Delete, Permissions.AuthClaims.Edit })]
        [HttpGet("Users/{id:Guid}")]
        public async Task<ActionResult<Response<UserDto>>> GetUserAsync(string id)
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

        [ClaimRequirement("Permission", new string[] { Permissions.AuthClaims.Create, Permissions.AuthClaims.View, Permissions.AuthClaims.Delete, Permissions.AuthClaims.Edit })]
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

        [HttpGet("Roles/Dropdown")]
        public async Task<ActionResult<Response<IEnumerable<Role>>>> GetRolesDropDown()
        {
            return Ok(await _userService.GetRolesDropDown()); // Status Code : 200
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
        private void SetTokenInCookie(string cookieName, string token, DateTime dateTime)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = dateTime,
                SameSite = SameSiteMode.None,
                Secure = true
            };
            Response.Cookies.Append(cookieName, token, cookieOptions);
        }
    }
}