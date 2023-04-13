using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Specifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services
{
    public class ApplicantService : IApplicantService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public ApplicantService(IUnitOfWork unitOfWork, IMapper mapper,
            UserManager<User> userManager, RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<PaginationResponse<List<ApplicantDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var result = await _unitOfWork.Applicant.GetAll(new ApplicantSpecs(filter, false));
            if (result.Count() == 0)
                return new PaginationResponse<List<ApplicantDto>>(_mapper.Map<List<ApplicantDto>>(result), "List is empty");

            var totalRecords = await _unitOfWork.Applicant.TotalRecord(new ApplicantSpecs(filter, true));
            return new PaginationResponse<List<ApplicantDto>>(_mapper.Map<List<ApplicantDto>>(result), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<ApplicantDto>> GetByIdAsync(int id)
        {
            var result = await _unitOfWork.Applicant.GetById(id, new ApplicantSpecs(false));
            if (result == null)
                return new Response<ApplicantDto>("Not found");

            return new Response<ApplicantDto>(_mapper.Map<ApplicantDto>(result), "Returning value");
        }

        public async Task<Response<List<ApplicantDto>>> GetDropDown()
        {
            var result = await _unitOfWork.Applicant.GetAll();
            if (!result.Any())
                return new Response<List<ApplicantDto>>(null, "List is empty");

            return new Response<List<ApplicantDto>>(_mapper.Map<List<ApplicantDto>>(result), "Returning List");
        }

        public async Task<Response<ApplicantDto>> CreateAsync(CreateApplicantDto entity)
        {
            var result = await _unitOfWork.Applicant.Add(_mapper.Map<Applicant>(entity));
            await _unitOfWork.SaveAsync();
            return new Response<ApplicantDto>(_mapper.Map<ApplicantDto>(result), "Created successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        //public async Task<Response<ApplicantDto>> UpdateAsync(CreateApplicantDto entity)
        //{
        //    var result = await _unitOfWork.Applicant.GetById((int)entity.Id, new ApplicantSpecs(true));
        //    if (result == null)
        //        return new Response<ApplicantDto>("Not found");

        //    //For updating data
        //    _mapper.Map(entity, result);
        //    await _unitOfWork.SaveAsync();
        //    return new Response<ApplicantDto>(_mapper.Map<ApplicantDto>(result), "Updated successfully");
        //}

        public Task<Response<ApplicantDto>> UpdateAsync(CreateApplicantDto entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<string>> LoginApplicant(LoginDto entity)
        {
            //Finding user by email
            var user = await _userManager.FindByEmailAsync(entity.Email);
            if (user == null)
                return new Response<string>("There is no user with that Email");

            //Checking user password
            var result = await _userManager.CheckPasswordAsync(user, entity.Password);
            if (!result)
                return new Response<string>("Invalid Password");

            if(user.ApplicantId == null)
                return new Response<string>("This user is not applicant");

            var applicantName = await _unitOfWork.Applicant.GetById((int)user.ApplicantId);

            //Getting user roles
            var userRoles = await _userManager.GetRolesAsync(user);

            //Declaring claims list
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, applicantName.Name == null ? "Naveed" : applicantName.Name),
                new Claim("Email", entity.Email),
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

            return new Response<string>(tokenAsString, "Login Successfully");
        }

        public async Task<Response<int>> RegisterApplicant(RegisterApplicantDto entity)
        {
            //Checking password
            if (entity.Password != entity.ConfirmPassword)
                return new Response<int>("Confirm password doesn't match with the password");

            //Checking Validations
            var checkingEmail = _unitOfWork.Applicant
                .Find(new ApplicantSpecs(entity.Email, 1)).FirstOrDefault();
            if (checkingEmail != null)
                return new Response<int>("Email already register");

            var checkingCnic = _unitOfWork.Applicant
                .Find(new ApplicantSpecs(entity.CNIC, 2)).FirstOrDefault();
            if (checkingCnic != null)
                return new Response<int>("CNIC already register");

            //Creating Transaction
            _unitOfWork.CreateTransaction();

            //Creating User
            var user = new User
            {
                Email = entity.Email,
                UserName = entity.Email
            };
            var userCreated = await _userManager.CreateAsync(user, entity.Password);
            if (!userCreated.Succeeded)
            {
                _unitOfWork.Rollback();
                return new Response<int>(userCreated.Errors.Select(e => e.Description).FirstOrDefault());
            }
            //Adding role for user
            var rolesAdded = await _userManager.AddToRoleAsync(user, Roles.Applicant.ToString());
            if (!rolesAdded.Succeeded)
            {
                _unitOfWork.Rollback();
                return new Response<int>(rolesAdded.Errors.Select(e => e.Description).FirstOrDefault());
            }

            //Creating Business Partner
            var businessPartner = new BusinessPartner(
                entity.Name,
                BusinessPartnerType.Student,
                entity.CNIC
                );
            await _unitOfWork.BusinessPartner.Add(businessPartner);
            await _unitOfWork.SaveAsync();


            //Creating Applicant
            var applicant = _mapper.Map<Applicant>(entity);
            applicant.SetBusinessPartnerId(businessPartner.Id);
            await _unitOfWork.Applicant.Add(applicant);
            await _unitOfWork.SaveAsync();

            //Adding Applicant Id
            user.ApplicantId = applicant.Id;
            var updateUser = await _userManager.UpdateAsync(user);
            if (!updateUser.Succeeded)
            {
                _unitOfWork.Rollback();
                return new Response<int>(updateUser.Errors.Select(e => e.Description).FirstOrDefault());
            }

            _unitOfWork.Commit();
            return new Response<int>(1, "Register Successfully");
        }

    }
}
