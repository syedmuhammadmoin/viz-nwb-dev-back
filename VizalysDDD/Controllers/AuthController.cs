using Application.Contracts.DTOs;
using Application.Contracts.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }
        // /api/auth/login
        [HttpPost("Login")]
        public async Task<ActionResult<LoginDto>> LoginAsync([FromBody] LoginDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.LoginUserAsync(model);

                if (result.IsSuccess)
                    return Ok(result);

                return BadRequest(result);
            }
            return BadRequest("Some properties are not valid");
        }

        //  /api/auth/RegisterUser
        [HttpPost("Users")]
        public async Task<ActionResult<RegisterUserDto>> RegisterUserAsync([FromBody] RegisterUserDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.RegisterUserAsync(model);

                if (result.IsSuccess)
                    return Ok(result); // Status Code : 200

                return BadRequest(result);
            }
            return BadRequest("Some properties are not valid"); // Status code : 400
        }

        //  /api/auth/GetUser
        [HttpGet("Users")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.GetUsersAsync();

                if (result.IsSuccess)
                    return Ok(result); // Status Code : 200

                return BadRequest(result);

            }
            return BadRequest("Some properties are not valid"); // Status code : 400
        }

        //  /api/auth/GetUserByID
        [HttpGet("Users/{id:Guid}")]
        public async Task<ActionResult<EditUserDto>> GetUserAsync(string id)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.GetUserAsync(id);

                if (result.IsSuccess)
                    return Ok(result); // Status Code : 200

                return BadRequest(result);

            }
            return BadRequest("Some properties are not valid"); // Status code : 400
        }

        // /api/auth/UpdateUser
        [HttpPut("Users/{id:Guid}")]
        public async Task<ActionResult<bool>> UpdateUserAsync(string id, EditUserDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.UpdateUserAsync(id, model);

                if (result.IsSuccess)
                    return Ok(result); // Status Code : 200

                return BadRequest(result);

            }
            return BadRequest("Some properties are not valid"); // Status code : 400
        }

        // /api/auth/User/ResetPass/id
        [HttpPut("Users/ResetPass/{id:Guid}")]
        public async Task<ActionResult<bool>> ResetUserPassword (string id, ResetPasswordDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.ResetUserPassword(id, model);

                if (result.IsSuccess)
                    return Ok(result); // Status Code : 200

                return BadRequest(result);

            }
            return BadRequest("Some properties are not valid"); // Status code : 400
        }

        // /api/auth/User/changePassword/id
        [HttpPut("Users/changePassword/{id:Guid}")]
        public async Task<ActionResult<bool>> ChangePassword(string id, ChangePasswordDto model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.ChangePassword(id, model);

                if (result.IsSuccess)
                    return Ok(result); // Status Code : 200

                return BadRequest(result);

            }
            return BadRequest("Some properties are not valid"); // Status code : 400
        }
    }
}