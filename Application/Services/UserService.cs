﻿using Application.Contracts.DTOs;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(UserManager<User> userManager, IUnitOfWork unitOfWork, IConfiguration configuration, ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _context = context;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _roleManager = roleManager;

        }

        // For Users
        public async Task<Response<bool>> LoginUserAsync(LoginDto model)
        {
            //Finding user by email
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new Response<bool>("There is no user with that Email");
            }

            //Checking user password
            var result = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!result)
                return new Response<bool>("Invalid Password");

            //Getting user roles
            var userRoles = await _userManager.GetRolesAsync(user);

            //Declaring claims list
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("Email", model.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            var permissions = new List<Claim>();
            //Getting user claims
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await _roleManager.FindByNameAsync(userRole);
                if (role != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    foreach (Claim roleClaim in roleClaims)
                    {
                        permissions.Add(roleClaim);
                    }
                }
            }
            //Removing duplicate claims from list
            //claims = claims.GroupBy(elem => elem.Value).Select(group => group.First()).ToList();
            permissions = permissions.GroupBy(e => e.Value).Select(g => g.First()).ToList();
            foreach (var perimission in permissions)
            {
                claims.Add(perimission);
            }

            //Creating Token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["AuthSettings:Issuer"],
                audience: _configuration["AuthSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(10),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

            return new Response<bool>(true, tokenAsString);
        }

        public async Task<Response<bool>> RegisterUserAsync(RegisterUserDto model)
        {
            // with rollback Transaction
            _unitOfWork.CreateTransaction();
            try
            {
                if (model == null)
                {
                    return new Response<bool>("Model is empty");
                }

                //Checking password
                if (model.Password != model.ConfirmPassword)
                    return new Response<bool>("Confirm password doesn't match with the password");

                //Registering user
                var user = new User
                {
                    Email = model.Email,
                    UserName = model.UserName,
                };

                var userCreated = await _userManager.CreateAsync(user, model.Password);

                if (!userCreated.Succeeded)
                {
                    _unitOfWork.Rollback();
                    return new Response<bool>(userCreated.Errors.Select(e => e.Description).FirstOrDefault());
                }
                //Adding roles for user
                var roles = await _userManager.GetRolesAsync(user);

                var rolesAdded = await _userManager.RemoveFromRolesAsync(user, roles);

                rolesAdded = await _userManager.AddToRolesAsync(user, model.UserRoles.Where(x => x.Selected).Select(y => y.RoleName));

                if (!rolesAdded.Succeeded)
                {
                    _unitOfWork.Rollback();
                    return new Response<bool>(rolesAdded.Errors.Select(e => e.Description).FirstOrDefault());
                }

                _unitOfWork.Commit();
                return new Response<bool>(true, "User created successfully");
            }

            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<bool>(ex.Message);
            }
        }

        public async Task<Response<IEnumerable<User>>> GetUsersAsync()
        {
            //Getting current user
            var currentUser = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            //Removing current user from list
            IEnumerable<User> users = await _userManager.Users.Where(a => a.Id != currentUser).ToListAsync();

            if (users == null)
            {
                return new Response<IEnumerable<User>>("User list cannot be found");
            }

            return new Response<IEnumerable<User>>(users, currentUser);
        }

        public async Task<Response<EditUserDto>> GetUserAsync(string id)
        {
            //Getting user by id
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return new Response<EditUserDto>("User not found");
            }

            //Declaring user roles list
            var viewModel = new List<UserRolesDto>();
            //Getting roles for user
            foreach (var role in _roleManager.Roles)
            {
                var userRolesViewModel = new UserRolesDto
                {
                    RoleName = role.Name
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }
                viewModel.Add(userRolesViewModel);
            }

            //Setting user model
            var model = new EditUserDto()
            {
                UserId = id,
                UserName = user.UserName,
                Email = user.Email,
                UserRole = viewModel
            };
            return new Response<EditUserDto>(model, "Returning Roles");
        }

        public async Task<Response<bool>> UpdateUserAsync(string id, EditUserDto model)
        {
            // with rollback Transaction
            _unitOfWork.CreateTransaction();
            try
            {
                //Finding user by id
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return new Response<bool>("User not found");
                }
                //Updating user details
                user.Email = model.Email;
                user.UserName = model.UserName;

                var updateUser = await _userManager.UpdateAsync(user);
                if (!updateUser.Succeeded)
                {
                    _unitOfWork.Rollback();
                    return new Response<bool>(updateUser.Errors.Select(e => e.Description).FirstOrDefault());
                }
                //Getting roles of user
                var roles = await _userManager.GetRolesAsync(user);
                // Removing all roles from user
                var updateRole = await _userManager.RemoveFromRolesAsync(user, roles);
                //Updating roles for user
                updateRole = await _userManager.AddToRolesAsync(user, model.UserRole.Where(x => x.Selected).Select(y => y.RoleName));

                if (!updateRole.Succeeded)
                {
                    _unitOfWork.Rollback();
                    return new Response<bool>(updateRole.Errors.Select(e => e.Description).FirstOrDefault());
                }

                _unitOfWork.Commit();
                return new Response<bool>(true, "User updated successfully");
            }

            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<bool>(ex.Message);
            }
        }

        public async Task<Response<bool>> ResetUserPassword(string id, ResetPasswordDto model)
        {
            // with rollback Transaction
            _unitOfWork.CreateTransaction();
            try
            {
                //Getting current user
                var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var current = await _userManager.FindByIdAsync(currentUserId);

                //Checking user password
                var checkCurrentUserPassword = await _userManager.CheckPasswordAsync(current, model.AdminPassword);
                if (!checkCurrentUserPassword)
                    return new Response<bool>("Invalid Password");

                //Checking input password
                if (model.Password != model.ConfirmPassword)
                    return new Response<bool>("Confirm password doesn't match the password");

                //Finding user by id
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return new Response<bool>("User not found");
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, model.Password);

                if (!result.Succeeded)
                {
                    return new Response<bool>("Something went wrong, contact your administrator");
                }

                _unitOfWork.Commit();
                return new Response<bool>(true, "Password reset successfully");
            }

            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<bool>(ex.Message);
            }
        }

        public async Task<Response<bool>> ChangePassword(string id, ChangePasswordDto model)
        {
            string username;
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                var authenticatedUserName = httpContext.User.Identity.Name;
                username = authenticatedUserName;
                // If it returns null, even when the user was authenticated, you may try to get the value of a specific claim 
                //var authenticatedUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                // var authenticatedUserId = _httpContextAccessor.HttpContext.User.FindFirst("sub").Value
            }
            username = "UserName";
            //Getting current user
            var cc = ClaimTypes.NameIdentifier.FirstOrDefault();
            var sss = _httpContextAccessor.HttpContext.User;
            var currentUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            // with rollback Transaction
            _unitOfWork.CreateTransaction();
            try
            {
                //if (id != currentUserId)
                //{
                //    return new Response<bool>("User id does not match with the current user id");
                //}
                //Getting user by id
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return new Response<bool>("User id does not match with the current user id");
                }

                //Checking current password
                var checkCurrentPassword = await _userManager.CheckPasswordAsync(user, model.CurrentPassword);
                if (!checkCurrentPassword)
                    return new Response<bool>("Current password not corrected");

                //Checking input password
                if (model.Password != model.ConfirmPassword)
                    return new Response<bool>("Confirm password doesn't match the password");

                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.Password);

                if (!result.Succeeded)
                {
                    return new Response<bool>("Something went wrong, contact your administrator");
                }

                _unitOfWork.Commit();
                return new Response<bool>(true, "Password changed successfully");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<bool>(ex.Message);
            }
        }
    }
}