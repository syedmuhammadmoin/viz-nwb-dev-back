using Application.Contracts.DTOs;
using Application.Contracts.Helper;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using Infrastructure.CustomUserManager;
using Infrastructure.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationUserManager _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailSenderService _emailSenderService;
        private readonly ApplicationDbContext _context;


        public UserService(ApplicationUserManager userManager, IUnitOfWork unitOfWork, IConfiguration configuration,
           ApplicationDbContext context, RoleManager<Role> roleManager, IMapper mapper, IHttpContextAccessor httpContextAccessor, IEmailSenderService emailSenderService)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _roleManager = roleManager;
            _mapper = mapper;
            _context = context;
            _emailSenderService = emailSenderService;

        }

        public async Task<Response<AuthenticationDto>> LoginUserSAAS(LoginSAASDto model)
        {
            //Finding user by email
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return new Response<AuthenticationDto>("There is no user with that Email");

            if (!user.EmailConfirmed)
                return new Response<AuthenticationDto>("Please confirmed your email first");

            //Checking user password
            var result = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!result)
                return new Response<AuthenticationDto>("Invalid Password");

            //Declaring claims list
            var claims = new List<Claim>
            {
                new Claim("Organization", "0"),
            };

            //Creating Token
            string tokenAsString = CreateJwtToken(claims, user, DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["AuthSettings:SaaSJWTDurationInMinutes"])));

            var authenticationDto = new AuthenticationDto()
            {
                Token = "N/A",
                SAASToken = tokenAsString,
                Email = model.Email,
                UserName = user.FirstName,
                DateFormat = user.DateFormat == null ? "MMM d, yy" : user.DateFormat,
                TokenExpiration = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["AuthSettings:SaaSJWTDurationInMinutes"]))
            };
            await AddRoleAndPermission(claims, user, authenticationDto);
            return new Response<AuthenticationDto>(authenticationDto, "Login successfully");
        }
        public async Task<Response<AuthenticationDto>> LoginUserAsync(LoginAPPDto model)
        {
            //Finding user by email
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return new Response<AuthenticationDto>("There is no user with that Email");

            if (!user.EmailConfirmed)
                return new Response<AuthenticationDto>("Please confirmed your email first");

            //Checking user password
            var result = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!result)
                return new Response<AuthenticationDto>("Invalid Password");

            //Declaring claims list
            var claims = new List<Claim>();

            int orgId;//for getting organization id
            string currency;//for getting organization currency

            if (model.OrganizationId != null && model.OrganizationId != 0)
            {
                var checkOrg = await _unitOfWork.Organization.GetById((int)model.OrganizationId);
                if (checkOrg == null)
                    return new Response<AuthenticationDto>("Invalid Organization");

                var checkUserOrganization = _unitOfWork.UsersOrganization.Find(new UsersOrganizationSpecs(user.Id, checkOrg.Id)).ToList();
                if (!checkUserOrganization.Any())
                    return new Response<AuthenticationDto>("No Organization find for this user");

                orgId = checkOrg.Id;
                currency = checkOrg.Currency;
                claims.Add(new Claim("Organization", orgId.ToString()));
                claims.Add(new Claim("Currency", String.IsNullOrEmpty(checkOrg.Currency) ? "$" : checkOrg.Currency));

                user.LastOrganizationAccess = checkOrg.Id;
                var updateUser = await _userManager.UpdateAsync(user);
                if (!updateUser.Succeeded)
                {
                    return new Response<AuthenticationDto>(updateUser.Errors.Select(e => e.Description).FirstOrDefault());
                }
            }
            else
            {

                if (user.LastOrganizationAccess == null)
                    return new Response<AuthenticationDto>("Please select organization");

                orgId = (int)user.LastOrganizationAccess;
                var org = await _unitOfWork.Organization.GetById(orgId);
                currency = org.Currency;
                claims.Add(new Claim("Organization", user.LastOrganizationAccess.ToString()));
                claims.Add(new Claim("Currency", String.IsNullOrEmpty(currency) ? "$" : currency));
            }

            var authenticationDto = new AuthenticationDto()
            {
                UserName = user.FirstName,
                Email = model.Email,
                OrganizationId = orgId,
                DateFormat = user.DateFormat == null ? "MMM d, yy" : user.DateFormat,
                Currency = currency
            };
            var res = await AddRoleAndPermission(claims, user, authenticationDto);
            if (!res)
            {
                return new Response<AuthenticationDto>("Login failed, please contact system administrator");
            }

            return new Response<AuthenticationDto>(authenticationDto, "Login successfully");
        }
        //public async Task<Response<bool>> RegisterUserAsync(RegisterUserDto model)
        //{
        //    //Checking password
        //    if (model.Password != model.ConfirmPassword)
        //        return new Response<bool>("Confirm password doesn't match with the password");

        //    var getEmployee = await _unitOfWork.Employee.GetById((int)model.EmployeeId);
        //    if (model.EmployeeId != getEmployee.Id)
        //        return new Response<bool>("Only Employees can be a user");

        //    //Checking if user alerady created for the employee
        //    var getUser = await _userManager.Users
        //        .FirstOrDefaultAsync(i => i.EmployeeId == model.EmployeeId);

        //    if (getUser != null)
        //        return new Response<bool>("User for this employee is already created");

        //    //Registering user
        //    var user = new User
        //    {
        //        EmployeeId = getEmployee.Id,
        //        Email = model.Email,
        //        UserName = model.Email
        //    };

        //    // with rollback Transaction
        //    _unitOfWork.CreateTransaction();
        //    var userCreated = await _userManager.CreateAsync(user, model.Password);
        //    if (!userCreated.Succeeded)
        //    {
        //        _unitOfWork.Rollback();
        //        return new Response<bool>(userCreated.Errors.Select(e => e.Description).FirstOrDefault());
        //    }

        //    //Adding roles for user
        //    var roles = await _userManager.GetRolesAsync(user);
        //    var rolesAdded = await _userManager.RemoveFromRolesAsync(user, roles);

        //    //Checking if user has selected any role
        //    if (model.UserRoles.Where(x => x.Selected).Count() < 1)
        //        return new Response<bool>("At least 1 role is required for creating User");

        //    rolesAdded = await _userManager.AddToRolesAsync(user, model.UserRoles.Where(x => x.Selected).Select(y => y.RoleName));
        //    if (!rolesAdded.Succeeded)
        //    {
        //        _unitOfWork.Rollback();
        //        return new Response<bool>(rolesAdded.Errors.Select(e => e.Description).FirstOrDefault());
        //    }

        //    _unitOfWork.Commit();
        //    return new Response<bool>(true, "User created successfully");
        //}
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

                //Checking for resending email verfication
                var checkingUser = await _userManager.FindByEmailAsync(model.Email);
                if (checkingUser != null)
                {
                    if (!checkingUser.EmailConfirmed)
                    {
                        checkingUser.FirstName = model.UserName;
                        var updateUser = await _userManager.UpdateAsync(checkingUser);
                        if (!updateUser.Succeeded)
                        {
                            _unitOfWork.Rollback();
                            return new Response<bool>(updateUser.Errors.Select(e => e.Description).FirstOrDefault());
                        }
                        var passResettoken = await _userManager.GeneratePasswordResetTokenAsync(checkingUser);

                        var result = await _userManager.ResetPasswordAsync(checkingUser, passResettoken, model.Password);
                        if (!result.Succeeded)
                        {
                            _unitOfWork.Rollback();
                            return new Response<bool>(result.Errors.Select(e => e.Description).FirstOrDefault());
                        }

                        // Email Sending to Users Email Address
                        if (!await GenerateEmailForConfirmation(checkingUser))
                        {
                            _unitOfWork.Rollback();
                            return new Response<bool>("Error in sending confirmation email");
                        }
                        _unitOfWork.Commit();
                        return new Response<bool>(true, "Send Confirmation Email");
                    }
                }

                //Registering user
                var user = new User
                {
                    Email = model.Email,
                    UserName = model.Email,
                    FirstName = model.UserName
                };

                var userCreated = await _userManager.CreateAsync(user, model.Password);
                await _unitOfWork.SaveAsync();

                if (!userCreated.Succeeded)
                    return new Response<bool>(userCreated.Errors.Select(e => e.Description).FirstOrDefault());

                //Checking User Email in Invite User Table
                var checkInviteuser = _unitOfWork.InviteUser
                .Find(new InviteUserSpecs(user.Email, true)).ToList();

                if (checkInviteuser.Count > 0)
                {
                    var usersOrganizations = new List<UsersOrganization>();
                    foreach (var org in checkInviteuser)
                    {
                        usersOrganizations.Add(new UsersOrganization(user.Id, org.OrgId));
                        org.IsDelete = true;

                        //adding role for user
                        var updateRole = await _userManager.AddToRoleByRoleIdAsync(user, org.RoleId);
                        if (!updateRole.Succeeded)
                        {
                            _unitOfWork.Rollback();
                            return new Response<bool>(updateRole.Errors.Select(e => e.Description).FirstOrDefault());
                        }
                        user.LastOrganizationAccess = org.OrgId;
                    }
                    await _unitOfWork.UsersOrganization.AddRange(usersOrganizations);
                    await _unitOfWork.SaveAsync();
                }
                //todo: use AWS cognito
                // Email Sending to Users Email Address
                if (!await GenerateEmailForConfirmation(user))
                {
                    _unitOfWork.Rollback();
                    return new Response<bool>("Error in sending confirmation email");
                }

                _unitOfWork.Commit();
                return new Response<bool>(true, "Send Confirmation Email");
            }

            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<bool>(ex.Message);
            }
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
            //Finding user by id
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return new Response<bool>("User not found");
            
            //Updating user details
            user.Email = model.Email;
            user.UserName = model.UserName;
            
            // with rollback Transaction
            _unitOfWork.CreateTransaction();
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
            var identityRole = new Role
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
            IEnumerable<Role> roles = await _roleManager.Roles
                .Where(i => i.Name != Roles.Applicant.ToString())
                .ToListAsync();
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
            if (role == null || role.Name == Roles.Applicant.ToString())
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
            allPermissions.GetPermissions(typeof(Permissions.BidEvaluationClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.QuotationClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.CallForQuotationClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.QuotationComparativeClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.DepreciationModelClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.FixedAssetClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.CWIPClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.DisposalClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.BudgetReappropriationClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.FixedAssetReportClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.DepreciationAdjustmentClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.FacultyClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.AcademicDepartmentClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.DegreeClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.ProgramClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.SemesterClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.CourseClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.QualificationClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.SubjectClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.FeeItemClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.CountryClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.StateClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.CityClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.DistrictClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.DomicileClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.DashboardProfitLossClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.DashboardBalanceSheetClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.DashboardBankBalanceClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.JournalClaims), id);
            allPermissions.GetPermissions(typeof(Permissions.TaxGroupClaims), id);


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

            if (role == null || role.Name == Roles.Applicant.ToString())
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

        public async Task<Response<IEnumerable<Role>>> GetRolesDropDown()
        {
            IEnumerable<Role> roles = await _roleManager.Roles
                .Where(i => i.Name != Roles.Applicant.ToString())
                .ToListAsync();
            if (roles == null)
            {
                return new Response<IEnumerable<Role>>("Role list cannot be found");
            }
            return new Response<IEnumerable<Role>>(roles, "Returning Roles");
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
            allPermissions.GetPermissions(typeof(Permissions.PettyCashClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.WorkflowStatusClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.WorkflowClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.ReceiptClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.GeneralLedgerClaims), "12");
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
            allPermissions.GetPermissions(typeof(Permissions.BidEvaluationClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.QuotationClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.CallForQuotationClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.QuotationComparativeClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.DepreciationModelClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.FixedAssetClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.CWIPClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.DisposalClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.BudgetReappropriationClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.DepreciationAdjustmentClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.FacultyClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.AcademicDepartmentClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.DegreeClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.ProgramClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.SemesterClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.CourseClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.QualificationClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.SubjectClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.FeeItemClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.CountryClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.StateClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.CityClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.DistrictClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.DomicileClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.DashboardBalanceSheetClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.DashboardProfitLossClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.DashboardBankBalanceClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.JournalClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.TaxGroupClaims), "12");
            allPermissions.GetPermissions(typeof(Permissions.TaxSettingClaims), "12");

            var allClaimValues = allPermissions.Select(a => a.Value).ToList();
            return new Response<List<string>>(allClaimValues, "Returning all claims");

        }
        private async Task<bool> GenerateEmailForConfirmation(User user)
        {
            // Email Sending to Users Email Address
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            if (token == null)
                return false;

            var uriBuilder = new UriBuilder(_configuration["ReturnPaths:ConfirmEmail"]);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["token"] = token;
            query["userid"] = user.Id;
            uriBuilder.Query = query.ToString();
            var urlString = uriBuilder.ToString();

            var senderEmail = _configuration["ReturnPaths:SenderEmail"];

            await _emailSenderService.SendEmailAsync(senderEmail, user.Email, "Confirm your email address", urlString);
            return true;
        }
        public async Task<bool> AddRoleAndPermission(List<Claim> claims, User user, AuthenticationDto authenticationDto)
        {
            //Getting user roles
            var userRoles = await _context.UserRoles.FromSqlRaw($"SELECT * FROM dbo.UserRoles WHERE UserId = '{user.Id}'").ToListAsync();

            var permissions = new List<Claim>();
            var roles = new List<string>();
            ////Getting user claims
            foreach (var userRole in userRoles)
            {
                var role = await _context.Roles.FromSqlRaw($"SELECT * FROM dbo.Roles WHERE Id = '{userRole.RoleId}'").IgnoreQueryFilters().FirstOrDefaultAsync();
                //var role = await _roleManager.FindByIdAsync(userRole.RoleId);
                if (role != null)
                {
                    if (role.OrganizationId == authenticationDto.OrganizationId)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role.Name));
                        roles.Add(role.Name);
                        var roleClaims = await _roleManager.GetClaimsAsync(role);
                        foreach (Claim roleClaim in roleClaims)
                        {
                            permissions.Add(roleClaim);
                        }
                    }
                }
            }

            ////Removing duplicate claims from list
            permissions = permissions.GroupBy(e => e.Value).Select(g => g.First()).ToList();
            foreach (var perimission in permissions)
            {
                claims.Add(perimission);
            }

            authenticationDto.Roles = roles;
            authenticationDto.Permissions = permissions.Select(i => i.Value).ToList();

            //var updateUser = await _userManager.UpdateAsync(user);
            if (user.RefreshTokens.Any(a => a.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.Where(a => a.IsActive == true).FirstOrDefault();
                authenticationDto.RefreshToken = activeRefreshToken.Token;
                authenticationDto.RefreshTokenExpiration = activeRefreshToken.Expires;
                claims.Add(new Claim("refreshTokenExpiry", activeRefreshToken.Expires.ToString()));
            }
            else
            {
                var refreshToken = CreateRefreshToken();
                authenticationDto.RefreshToken = refreshToken.Token;
                authenticationDto.RefreshTokenExpiration = refreshToken.Expires;
                claims.Add(new Claim("refreshTokenExpiry", refreshToken.Expires.ToString()));
                user.RefreshTokens.Add(refreshToken);

                var updateUserToken = await _userManager.UpdateAsync(user);
                if (!updateUserToken.Succeeded)
                    return false;
            }

            //Creating Token
            string tokenAsString = CreateJwtToken(claims, user, DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["AuthSettings:JWTDurationInMinutes"])));
            authenticationDto.Token = tokenAsString;

            return true;
        }
        public string CreateJwtToken(List<Claim> claims, User user, DateTime dateTime)
        {
            //Declaring claims list
            claims.AddRange(new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim("Email", user.Email),
                new Claim("DateFormat", user.DateFormat == null ? "MMM d, yy" : user.DateFormat),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            });

            //Creating Token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));

            var newtoken = new JwtSecurityToken(
                issuer: _configuration["AuthSettings:Issuer"],
                audience: _configuration["AuthSettings:Audience"],
                claims: claims,
                expires: dateTime,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(newtoken);
        }

        public async Task<Response<AuthenticationDto>> ChangeOrganization(int orgId)
        {
            // Getting Users from token
            var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Finding user
            var user = await _userManager.FindByIdAsync(currentUserId);

            if (user == null)
                return new Response<AuthenticationDto>("There is no user with that Id");


            //Declaring claims list
            var claims = new List<Claim>();

            if (orgId == 0)
                return new Response<AuthenticationDto>("Organization cannot be zero");

            var checkOrg = await _unitOfWork.Organization.GetById(orgId);
            if (checkOrg == null)
                return new Response<AuthenticationDto>("Invalid Organization");

            var checkUserOrganization = _unitOfWork.UsersOrganization.Find(new UsersOrganizationSpecs(user.Id, checkOrg.Id)).ToList();
            if (!checkUserOrganization.Any())
                return new Response<AuthenticationDto>("No Organization find for this user");

            claims.Add(new Claim("Organization", checkOrg.Id.ToString()));
            claims.Add(new Claim("Currency", String.IsNullOrEmpty(checkOrg.Currency) ? "$" : checkOrg.Currency));
            user.LastOrganizationAccess = checkOrg.Id;
            var updateUser = await _userManager.UpdateAsync(user);
            if (!updateUser.Succeeded)
            {
                return new Response<AuthenticationDto>(updateUser.Errors.Select(e => e.Description).FirstOrDefault());
            }
            var authenticationDto = new AuthenticationDto()
            {
                UserName = user.FirstName,
                Email = user.Email,
                OrganizationId = checkOrg.Id,
                DateFormat = user.DateFormat == null ? "MMM d, yy" : user.DateFormat,
                Currency = checkOrg.Currency
            };
            var res = await AddRoleAndPermission(claims, user, authenticationDto);

            if (!res)
            {
                return new Response<AuthenticationDto>("Login failed, please contact system administrator");
            }


            return new Response<AuthenticationDto>(authenticationDto, "Orgnization switch successfully");
        }
        //private methods 
        private RefreshToken CreateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(randomNumber);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomNumber),
                    Expires = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["AuthSettings:RefreshTokenDurationInMinutes"])),
                    Created = DateTime.UtcNow
                };
            }
        }


    }
}