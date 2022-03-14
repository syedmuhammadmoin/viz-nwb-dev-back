using Application.Contracts.DTOs;
using Application.Contracts.Response;
using Domain.Entities;
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
        Task<Response<bool>> CreateRoleAsync(RegisterRoleDto model);
        Task<Response<bool>> GetRolesAsync();
        Task<Response<bool>> GetRoleAsync(string id);
        Task<Response<bool>> UpdateRoleAsync(string id, RegisterRoleDto model);

        //FOR CLAIMS 
        Response<List<string>> GetClaimsAsync();
    }
}