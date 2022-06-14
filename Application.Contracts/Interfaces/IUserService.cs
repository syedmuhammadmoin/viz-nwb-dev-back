using Application.Contracts.DTOs;
using Application.Contracts.Response;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Interfaces
{
    public interface IUserService
    {
        //FOR USERS
        Task<Response<bool>> LoginUserAsync(LoginDto model);
        Task<Response<bool>> RegisterUserAsync(RegisterUserDto model);
        Task<Response<IEnumerable<User>>> GetUsersAsync();
        Task<Response<EditUserDto>> GetUserAsync(string id);
        Task<Response<bool>> UpdateUserAsync(string id, EditUserDto data);
        Task<Response<bool>> ResetUserPassword(string id, ResetPasswordDto data);
        Task<Response<bool>> ChangePassword(string id, ChangePasswordDto model);

        //FOR ROLES
        Task<Response<string>> CreateRoleAsync(RegisterRoleDto model);
        Task<Response<IEnumerable<IdentityRole>>> GetRolesAsync();
        Task<Response<IEnumerable<IdentityRole>>> GetRolesDropDown();
        Task<Response<RegisterRoleDto>> GetRoleAsync(string id);
        Task<Response<RegisterRoleDto>> UpdateRoleAsync(string id, RegisterRoleDto model);

        //FOR CLAIMS 
        Response<List<string>> GetClaimsAsync();
    }
}