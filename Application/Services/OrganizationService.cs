using Application.Contracts.DTOs;
using Application.Contracts.DTOs.Organizations;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.CustomUserManager;
using Infrastructure.Seeds;
using Infrastructure.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ApplicationUserManager _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly RoleManager<Role> _roleManager;

        public OrganizationService(IUnitOfWork unitOfWork, IMapper mapper, ApplicationUserManager userManager, IHttpContextAccessor httpContextAccessor, RoleManager<Role> roleManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _roleManager = roleManager;
        }
        public async Task<Response<OrganizationDto>> CreateAsync(CreateOrganizationDto entity, string token)
        {
            _unitOfWork.CreateTransaction();
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token);
                var tokenS = jsonToken as JwtSecurityToken;

                //Finding user by email
                var currentUserId = tokenS.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
                entity.UserId = currentUserId;

                //creating organization
                var org = _mapper.Map<Organization>(entity);
                var result = await _unitOfWork.Organization.Add(org);
                await _unitOfWork.SaveAsync();

                //adding in user organization table
                var userOrganization = await _unitOfWork.UsersOrganization.Add(new UsersOrganization(currentUserId, result.Id));
                await _unitOfWork.SaveAsync();

                
                //Taking organization as last access 
                var currentUser = await _userManager.FindByIdAsync(currentUserId);
                currentUser.LastOrganizationAccess = result.Id;
                await _userManager.UpdateAsync(currentUser);
                await _unitOfWork.SaveAsync();

                //adding user, roles and permission in organization
                var role = await DefaultRoles.SeedAsync(_roleManager, org.Id);
                await Seeding.COASeeds(_unitOfWork, result.Id);
                var user = await _userManager.FindByIdAsync(currentUserId);

                //updating user lastOrganizationAccessId
                user.LastOrganizationAccess = result.Id;
                await _userManager.AddToRoleByRoleIdAsync(user, role.Id);
                //await _userManager.AddToRoleAsync(user, Roles.SuperAdmin.ToString());
                await DefaultUsers.SeedSuperAdminAsync(_roleManager, role);


                //Updating user details
                var updateUser = await _userManager.UpdateAsync(user);
                if (!updateUser.Succeeded)
                {
                    _unitOfWork.Rollback();
                    return new Response<OrganizationDto>(updateUser.Errors.Select(e => e.Description).FirstOrDefault());
                }
                //Commiting the transaction 
                _unitOfWork.Commit();

                return new Response<OrganizationDto>(_mapper.Map<OrganizationDto>(result), "Created successfully");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<OrganizationDto>(ex.Message);
            }
        }
        public async Task<Response<OrganizationDto>> Create2Async(CreateOrganizationDto entity)
        {
            _unitOfWork.CreateTransaction();
            try
            {
                var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value; 
                //var handler = new JwtSecurityTokenHandler();
                //var jsonToken = handler.ReadToken(token);
                //var tokenS = jsonToken as JwtSecurityToken;

                ////Finding user by email
                //var currentUserId = tokenS.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
                //entity.UserId = currentUserId;

                //creating organization
                var org = _mapper.Map<Organization>(entity);
                var result = await _unitOfWork.Organization.Add(org);
                await _unitOfWork.SaveAsync();

                //adding in user organization table
                var userOrganization = await _unitOfWork.UsersOrganization.Add(new UsersOrganization(currentUserId, result.Id));
                await _unitOfWork.SaveAsync();


                //Taking organization as last access 
                var currentUser = await _userManager.FindByIdAsync(currentUserId);
                currentUser.LastOrganizationAccess = result.Id;
                await _userManager.UpdateAsync(currentUser);
                await _unitOfWork.SaveAsync();

                //adding user, roles and permission in organization
                var role = await DefaultRoles.SeedAsync(_roleManager, org.Id);
                await Seeding.COASeeds(_unitOfWork, result.Id);
                var user = await _userManager.FindByIdAsync(currentUserId);

                //updating user lastOrganizationAccessId
                user.LastOrganizationAccess = result.Id;
                await _userManager.AddToRoleByRoleIdAsync(user, role.Id);
                //await _userManager.AddToRoleAsync(user, Roles.SuperAdmin.ToString());
                await DefaultUsers.SeedSuperAdminAsync(_roleManager, role);


                //Updating user details
                var updateUser = await _userManager.UpdateAsync(user);
                if (!updateUser.Succeeded)
                {
                    _unitOfWork.Rollback();
                    return new Response<OrganizationDto>(updateUser.Errors.Select(e => e.Description).FirstOrDefault());
                }
                //Commiting the transaction 
                _unitOfWork.Commit();

                return new Response<OrganizationDto>(_mapper.Map<OrganizationDto>(result), "Created successfully");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new Response<OrganizationDto>(ex.Message);
            }
        }
        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PaginationResponse<List<OrganizationDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var specification = new OrganizationSpecs(filter, false);
            var orgs = await _unitOfWork.Organization.GetAll(specification);

            if (orgs.Count() == 0)
                return new PaginationResponse<List<OrganizationDto>>(_mapper.Map<List<OrganizationDto>>(orgs), "List is empty");

            var totalRecords = await _unitOfWork.Organization.TotalRecord();

            return new PaginationResponse<List<OrganizationDto>>(_mapper.Map<List<OrganizationDto>>(orgs),
                filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<List<OrganizationDto>>> GetOrganizationDropDown()
        {
            var organizations = await _unitOfWork.Organization.GetAll();
            if (!organizations.Any())
                return new Response<List<OrganizationDto>>("List is empty");

            return new Response<List<OrganizationDto>>(_mapper.Map<List<OrganizationDto>>(organizations), "Returning List");
        }

        public async Task<Response<OrganizationDto>> GetByIdAsync(int id)
        {
            var org = await _unitOfWork.Organization.GetById(id);
            if (org == null)
                return new Response<OrganizationDto>("Not found");

            return new Response<OrganizationDto>(_mapper.Map<OrganizationDto>(org), "Returning value");
        }

        public async Task<Response<OrganizationDto>> UpdateAsync(UpdateOrganizationDto entity)
        {
            var org = await _unitOfWork.Organization.GetById((int)entity.Id);

            if (org == null)
                return new Response<OrganizationDto>("Not found");

            //For updating data
            _mapper.Map<UpdateOrganizationDto, Organization>(entity, org);
            await _unitOfWork.SaveAsync();
            return new Response<OrganizationDto>(_mapper.Map<OrganizationDto>(org), "Updated successfully");
        }
        public async Task<Response<List<UsersOrganizationDto>>> GetOrganizationByUserId()
        {
            try
            {
                // Getting Users
                var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _userManager.FindByIdAsync(currentUserId);
                if (user == null)
                    return new Response<List<UsersOrganizationDto>>("User Not Found");

                var result = _unitOfWork.UsersOrganization.Find(new UsersOrganizationSpecs(user.Id)).ToList();
                return new Response<List<UsersOrganizationDto>>(_mapper.Map<List<UsersOrganizationDto>>(result), "Returning value");

            }
            catch (Exception ex)
            {
                return new Response<List<UsersOrganizationDto>>(ex.Message);
            }
        }
        public Task<Response<OrganizationDto>> CreateAsync(CreateOrganizationDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
