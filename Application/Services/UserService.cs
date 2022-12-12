using Application.Contracts.DTOs;
using Application.Contracts.Helper;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
using Domain.Constants;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(UserManager<User> userManager, IUnitOfWork unitOfWork, IConfiguration configuration,
            RoleManager<IdentityRole> roleManager, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _roleManager = roleManager;
            _mapper = mapper;
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
            var employeeName = await _userManager.Users
                                        .Include(i => i.Employee)
                                        .Where(i => i.Id == user.Id)
                                        .Select(i => i.Employee.Name)
                                        .FirstOrDefaultAsync();

            //Checking user password
            var result = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!result)
                return new Response<bool>("Invalid Password");

            //Getting user roles
            var userRoles = await _userManager.GetRolesAsync(user);

            //Declaring claims list
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, employeeName == null ? "Naveed" : employeeName),
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
            if (model == null)
            {
                return new Response<bool>("Model is empty");
            }

            var getEmployee = await _unitOfWork.Employee.GetById((int)model.EmployeeId);

            if (model.EmployeeId != getEmployee.Id)
            {
                return new Response<bool>("Only Employees can be a user");
            }

            //Checking password
            if (model.Password != model.ConfirmPassword)
                return new Response<bool>("Confirm password doesn't match with the password");


            //Registering user
            var user = new User
            {
                EmployeeId = getEmployee.Id,
                Email = model.Email,
                UserName = model.Email
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

            //Checking if user has selected any role
            if (model.UserRoles.Where(x => x.Selected).Count() < 1)
                return new Response<bool>("At least 1 role is required for creating User");

            rolesAdded = await _userManager.AddToRolesAsync(user, model.UserRoles.Where(x => x.Selected).Select(y => y.RoleName));

            if (!rolesAdded.Succeeded)
            {
                _unitOfWork.Rollback();
                return new Response<bool>(rolesAdded.Errors.Select(e => e.Description).FirstOrDefault());
            }

            _unitOfWork.Commit();
            return new Response<bool>(true, "User created successfully");

        }

        public async Task<Response<IEnumerable<UsersListDto>>> GetUsersAsync()
        {
            //Getting current user
            var currentUser = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            //Removing current user from list
            IEnumerable<User> users = await _userManager.Users.Where(a => a.Id != currentUser).Include(a => a.Employee).ToListAsync();

            if (users == null)
            {
                return new Response<IEnumerable<UsersListDto>>("User list cannot be found");
            }

            return new Response<IEnumerable<UsersListDto>>(_mapper.Map<List<UsersListDto>>(users), currentUser);
        }

        public async Task<Response<UserDto>> GetUserAsync(string id)
        {
            //Getting user by id
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return new Response<UserDto>("User not found");
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
            var model = new UserDto()
            {
                EmployeeId = user.EmployeeId,
                UserId = id,
                UserName = user.UserName,
                Email = user.Email,
                UserRoles = viewModel
            };
            return new Response<UserDto>(model, "Returning Roles");
        }

        public async Task<Response<bool>> UpdateUserAsync(string id, EditUserDto model)
        {
            // with rollback Transaction
            _unitOfWork.CreateTransaction();

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

            //Checking if user has selected any role
            if (model.UserRoles.Where(x => x.Selected).Count() < 1)
                return new Response<bool>("At least 1 role is required for creating User");

            //Updating roles for user
            updateRole = await _userManager.AddToRolesAsync(user, model.UserRoles.Where(x => x.Selected).Select(y => y.RoleName));

            if (!updateRole.Succeeded)
            {
                _unitOfWork.Rollback();
                return new Response<bool>(updateRole.Errors.Select(e => e.Description).FirstOrDefault());
            }

            _unitOfWork.Commit();
            return new Response<bool>(true, "User updated successfully");

        }

        public async Task<Response<bool>> ResetUserPassword(string id, ResetPasswordDto model)
        {
            // with rollback Transaction
            _unitOfWork.CreateTransaction();

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

        //For Roles
        public async Task<Response<string>> CreateRoleAsync(RegisterRoleDto model)
        {
            if (model == null)
            {
                return new Response<string>("Role model is null");
            }

            // with rollback Transaction
            _unitOfWork.CreateTransaction();

            //Add role in identity Role table
            var identityRole = new IdentityRole
            {
                Name = model.RoleName
            };
            var createRole = await _roleManager.CreateAsync(identityRole); ;

            //If role not created successfully
            if (!createRole.Succeeded)
                return new Response<string>("Role did not create");

            //Getting and removing all claims for this role
            var claims = await _roleManager.GetClaimsAsync(identityRole);
            foreach (var claim in claims)
            {
                await _roleManager.RemoveClaimAsync(identityRole, claim);
            }
            //Add selected claims in role
            var selectedClaims = model.RoleClaims.Where(a => a.Selected).ToList();
            foreach (var claim in selectedClaims)
            {
                await _roleManager.AddPermissionClaim(identityRole, claim.Value);
            }

            _unitOfWork.Commit();
            return new Response<string>(identityRole.ToString(), "Role created successfully");

        }

        public async Task<Response<IEnumerable<IdentityRole>>> GetRolesAsync()
        {
            IEnumerable<IdentityRole> roles = await _roleManager.Roles.ToListAsync();
            if (roles == null)
            {
                return new Response<IEnumerable<IdentityRole>>("Role list cannot be found");
            }
            return new Response<IEnumerable<IdentityRole>>(roles, "Returning Roles");
        }

        public async Task<Response<RegisterRoleDto>> GetRoleAsync(string id)
        {
            //Getting role by id
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return new Response<RegisterRoleDto>("Cannot find role with the input id");

            var allPermissions = new List<RegisterRoleClaimsDto>();
            allPermissions.GetPermissions(typeof(Permissions.AuthClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.BusinessPartnerClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.OrganizationClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.DepartmentClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.DesignationClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.WarehouseClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.LocationClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.BankAccountClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.BankStatementClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.CashAccountClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.ChartOfAccountClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.CampusClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.CategoriesClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.ProductsClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.Level4Claims), id);
            allPermissions.GetPermissions(typeof(Permissions.BankReconClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.TransactionReconClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.InvoiceClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.BillClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.PaymentClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.CreditNoteClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.DebitNoteClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.JournalEntryClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.WorkflowStatusClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.WorkflowClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.EstimatedBudgetClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.BudgetClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.ReceiptClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.GeneralLedgerClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.TrialBalanceClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.BalanceSheetClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.ProfitLossClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.BudgetReportClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.EmployeeClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.PayrollItemClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.PayrollTransactionClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.PayrollPaymentClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.TaxesClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.UnitOfMeasurementClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.IssuanceClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.GRNClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.PurchaseOrderClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.RequisitionClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.StockClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.GoodsReturnNoteClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.IssuanceReturnClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.RequestClaims), id);


            //Getting all claims for this role
            var claims = await _roleManager.GetClaimsAsync(role);
            //Selecting claims value
            var roleClaimValues = claims.Select(a => a.Value).ToList();
            //Getting all claims from system
            var allClaimValues = allPermissions.Select(a => a.Value).ToList();
            //intersecting authorized claims valus
            var authorizedClaims = allClaimValues.Intersect(roleClaimValues).ToList();
            // setting isSelected true for authorized claims
            foreach (var permission in allPermissions)
            {
                if (authorizedClaims.Any(a => a == permission.Value))
                {
                    permission.Selected = true;
                }
            }
            //mapping to role model
            var model = new RegisterRoleDto();
            model.RoleName = role.Name;
            model.RoleClaims = allPermissions;
            return new Response<RegisterRoleDto>(model, "Returning Roles");
        }

        public async Task<Response<RegisterRoleDto>> UpdateRoleAsync(string id, RegisterRoleDto model)
        {
            //Getting role by id
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
                return new Response<RegisterRoleDto>("Role not found");

            // with rollback Transaction
            _unitOfWork.CreateTransaction();

            //updating role values
            role.Name = model.RoleName;
            var updateRole = await _roleManager.UpdateAsync(role);

            if (!updateRole.Succeeded)
            {
                _unitOfWork.Rollback();
                return new Response<RegisterRoleDto>(updateRole.Errors.Select(e => e.Description).FirstOrDefault());
            }
            //getting and removing all claims for this role
            var claims = await _roleManager.GetClaimsAsync(role);
            foreach (var claim in claims)
            {
                await _roleManager.RemoveClaimAsync(role, claim);
            }
            //adding selecting claims in this role
            var selectedClaims = model.RoleClaims.Where(a => a.Selected).ToList();
            foreach (var claim in selectedClaims)
            {
                await _roleManager.AddPermissionClaim(role, claim.Value);
            }

            _unitOfWork.Commit();
            return new Response<RegisterRoleDto>(model, "Role updated successfully");


        }

        public async Task<Response<IEnumerable<IdentityRole>>> GetRolesDropDown()
        {
            IEnumerable<IdentityRole> roles = await _roleManager.Roles.ToListAsync();
            if (roles == null)
            {
                return new Response<IEnumerable<IdentityRole>>("Role list cannot be found");
            }
            return new Response<IEnumerable<IdentityRole>>(roles, "Returning Roles");
        }

        //For Claims
        public Response<List<string>> GetClaimsAsync()
        {
            var allPermissions = new List<RegisterRoleClaimsDto>();
            allPermissions.GetPermissions(typeof(Permissions.AuthClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.BusinessPartnerClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.OrganizationClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.DepartmentClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.DesignationClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.WarehouseClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.LocationClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.BankAccountClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.BankStatementClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.CashAccountClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.ChartOfAccountClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.CampusClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.CategoriesClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.ProductsClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.Level4Claims), "12");
            allPermissions.GetPermissions(typeof(Permissions.BankReconClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.TransactionReconClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.InvoiceClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.BillClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.PaymentClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.CreditNoteClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.DebitNoteClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.JournalEntryClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.WorkflowStatusClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.WorkflowClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.ReceiptClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.BudgetClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.EstimatedBudgetClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.TrialBalanceClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.BalanceSheetClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.ProfitLossClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.BudgetReportClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.EmployeeClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.PayrollItemClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.PayrollTransactionClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.PayrollPaymentClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.TaxesClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.UnitOfMeasurementClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.IssuanceClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.GRNClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.PurchaseOrderClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.RequisitionClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.StockClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.GoodsReturnNoteClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.IssuanceReturnClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.RequestClaims), "12");

            var allClaimValues = allPermissions.Select(a => a.Value).ToList();
            return new Response<List<string>>(allClaimValues, "Returning all claims");

        }
    }
}