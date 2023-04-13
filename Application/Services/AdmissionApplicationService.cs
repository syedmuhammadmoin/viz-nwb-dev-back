using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Application.Services
{
    public class AdmissionApplicationService : IAdmissionApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;

        public AdmissionApplicationService(IUnitOfWork unitOfWork, IMapper mapper,
            IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<PaginationResponse<List<AdmissionApplicationDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            //Getting current user
            var currentUser = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var getUser = await _userManager.FindByIdAsync(currentUser);
            if (getUser == null)
                return new PaginationResponse<List<AdmissionApplicationDto>>("Invalid User Id");

            if (getUser.ApplicantId != null)
            {
                var result = await _unitOfWork.AdmissionApplication.GetAll(new AdmissionApplicationSpecs(filter, false, (int)getUser.ApplicantId));
                if (result.Count() == 0)
                    return new PaginationResponse<List<AdmissionApplicationDto>>(_mapper.Map<List<AdmissionApplicationDto>>(result), "List is empty");

                var totalRecords = await _unitOfWork.AdmissionApplication.TotalRecord(new AdmissionApplicationSpecs(filter, true, (int)getUser.ApplicantId));
                return new PaginationResponse<List<AdmissionApplicationDto>>(_mapper.Map<List<AdmissionApplicationDto>>(result), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
            }
            else
            {
                var result = await _unitOfWork.AdmissionApplication.GetAll(new AdmissionApplicationSpecs(filter, false));
                if (result.Count() == 0)
                    return new PaginationResponse<List<AdmissionApplicationDto>>(_mapper.Map<List<AdmissionApplicationDto>>(result), "List is empty");

                var totalRecords = await _unitOfWork.AdmissionApplication.TotalRecord(new AdmissionApplicationSpecs(filter, true));
                return new PaginationResponse<List<AdmissionApplicationDto>>(_mapper.Map<List<AdmissionApplicationDto>>(result), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
            }
        }

        public async Task<Response<AdmissionApplicationDto>> GetByIdAsync(int id)
        {
            //Getting current user
            var currentUser = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var getUser = await _userManager.FindByIdAsync(currentUser);
            if (getUser == null)
                return new Response<AdmissionApplicationDto>("Invalid User Id");

            var result = await _unitOfWork.AdmissionApplication
                .GetById(id, getUser.ApplicantId != null 
                ? new AdmissionApplicationSpecs((int)getUser.ApplicantId, false)
                : new AdmissionApplicationSpecs());
            
            if (result == null)
                return new Response<AdmissionApplicationDto>("Not found");

            return new Response<AdmissionApplicationDto>(_mapper.Map<AdmissionApplicationDto>(result), "Returning value");
        }

        public async Task<Response<List<AdmissionApplicationDto>>> GetDropDown()
        {
            var result = await _unitOfWork.AdmissionApplication.GetAll();
            if (!result.Any())
                return new Response<List<AdmissionApplicationDto>>(null, "List is empty");

            return new Response<List<AdmissionApplicationDto>>(_mapper.Map<List<AdmissionApplicationDto>>(result), "Returning List");
        }

        public async Task<Response<AdmissionApplicationDto>> CreateAsync(CreateAdmissionApplicationDto entity)
        {
            //Getting current user
            var currentUser = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var getUser = await _userManager.FindByIdAsync(currentUser);
            if (getUser == null)
                return new Response<AdmissionApplicationDto>("Invalid User, please login as applicant");

            var getApplicant = await _unitOfWork.Applicant
                .GetById((int)getUser.ApplicantId, new ApplicantSpecs(true));
            if (getApplicant == null)
                return new Response<AdmissionApplicationDto>("Invalid applicant id");

            //Begin Transaction
            _unitOfWork.CreateTransaction();

            //For updating data
            _mapper.Map(entity, getApplicant);
            await _unitOfWork.SaveAsync();

            var result = await _unitOfWork.AdmissionApplication.Add(_mapper.Map<AdmissionApplication>(entity));
            await _unitOfWork.SaveAsync();
            _unitOfWork.Commit(); 
            return new Response<AdmissionApplicationDto>(_mapper.Map<AdmissionApplicationDto>(result), "Created successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<AdmissionApplicationDto>> UpdateAsync(CreateAdmissionApplicationDto entity)
        {
            //Getting current user
            var currentUser = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var getUser = await _userManager.FindByIdAsync(currentUser);
            if (getUser == null)
                return new Response<AdmissionApplicationDto>("Invalid User, please login as applicant");

            var getApplicant = await _unitOfWork.Applicant
                .GetById((int)getUser.ApplicantId, new ApplicantSpecs(true));
            if (getApplicant == null)
                return new Response<AdmissionApplicationDto>("Invalid applicant id");

            //Begin Transaction
            _unitOfWork.CreateTransaction();

            //For updating data
            _mapper.Map(entity, getApplicant);
            await _unitOfWork.SaveAsync();

            var result = await _unitOfWork.AdmissionApplication
                .GetById((int)entity.Id, new AdmissionApplicationSpecs(getApplicant.Id, true));
            if (result == null)
                return new Response<AdmissionApplicationDto>("Application Not found");

            //For updating data
            _mapper.Map(entity, result);
            await _unitOfWork.SaveAsync();
            return new Response<AdmissionApplicationDto>(_mapper.Map<AdmissionApplicationDto>(result), "Updated successfully");
        }

    }
}
