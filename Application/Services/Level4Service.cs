using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Helper;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Specifications;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class Level4Service : ILevel4Service
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Level4Service(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;

        }
        public async Task<Response<Level4Dto>> CreateAsync(CreateLevel4Dto entity)
        {
            var checkingCode = _unitOfWork.Level4.Find(new Level4Specs(entity.Code)).FirstOrDefault();
            if (checkingCode != null)
                return new Response<Level4Dto>("Duplicate code");

            var level3 = _unitOfWork.Level3.Find(new Level3Specs(entity.Level3_id)).FirstOrDefault();
            if (level3 == null)
            {
                return new Response<Level4Dto>("Invalid Level3 Account");
            }

            var level4 = _mapper.Map<Level4>(entity);
            level4.SetLevel1Id(level3.Level2.Level1_id);
            var result = await _unitOfWork.Level4.Add(level4);
            await _unitOfWork.SaveAsync();
            return new Response<Level4Dto>(_mapper.Map<Level4Dto>(result), "Created successfully");
        }

        public async Task<PaginationResponse<List<Level4Dto>>> GetAllAsync(Level4Filter filter)
        {

            var specification = new Level4Specs(filter);
            var level4 = await _unitOfWork.Level4.GetAll(specification);

            if (level4.Count() == 0)
                return new PaginationResponse<List<Level4Dto>>(_mapper.Map<List<Level4Dto>>(level4), "List is empty");

            var totalRecords = await _unitOfWork.Level4.TotalRecord();

            return new PaginationResponse<List<Level4Dto>>(_mapper.Map<List<Level4Dto>>(level4), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");

        }

        public async Task<Response<Level4Dto>> GetByIdAsync(string id)
        {
            var specification = new Level4Specs();
            var level4 = await _unitOfWork.Level4.GetById(id, specification);
            if (level4 == null)
                return new Response<Level4Dto>("Not found");

            return new Response<Level4Dto>(_mapper.Map<Level4Dto>(level4), "Returning value");
        }

        public async Task<Response<Level4Dto>> UpdateAsync(CreateLevel4Dto entity)
        {
            var level4 = await _unitOfWork.Level4.GetById(entity.Id);
            if (level4 == null)
                return new Response<Level4Dto>("Not found");

            var checkingCode = _unitOfWork.Level4.Find(new Level4Specs(entity.Code, entity.Id)).FirstOrDefault();
            if (checkingCode != null)
                return new Response<Level4Dto>("Duplicate code");

            
            //if(level4.AccountType == Domain.Constants.AccountType.SystemDefined)
            //    return new Response<Level4Dto>("System defined accounts cannot be edited");
            
            //For updating data
            _mapper.Map<CreateLevel4Dto, Level4>(entity, level4);
            await _unitOfWork.SaveAsync();
            return new Response<Level4Dto>(_mapper.Map<Level4Dto>(level4), "Updated successfully");
        }
        
        public async Task<Response<string>> DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<List<Level4Dto>>> GetLevel4DropDown()
        {
            var level4 = await _unitOfWork.Level4.GetAll();
            if (!level4.Any())
                return new Response<List<Level4Dto>>(null, "List is empty");

            return new Response<List<Level4Dto>>(_mapper.Map<List<Level4Dto>>(level4), "Returning List");

        }

        public async Task<Response<List<Level4Dto>>> GetAllOtherAccounts()
        {
            var level4 = await _unitOfWork.Level4.GetAll(new Level4Specs(0));
            if (!level4.Any())
                return new Response<List<Level4Dto>>(null, "List is empty");

            return new Response<List<Level4Dto>>(_mapper.Map<List<Level4Dto>>(level4), "Returning List");

        }

        public async Task<Response<List<Level4Dto>>> GetBudgetAccounts()
        {//SBBU-Code
            //var level4 = await _unitOfWork.Level4.GetAll(new Level4Specs(true));
            var level4 = await _unitOfWork.Level4.GetAll();
            if (!level4.Any())
                return new Response<List<Level4Dto>>(null, "List is empty");

            return new Response<List<Level4Dto>>(_mapper.Map<List<Level4Dto>>(level4), "Returning List");

        }

        public async Task<Response<List<Level4Dto>>> GetPayableAccounts()
        {//SBBU-Code
            //var level4 = await _unitOfWork.Level4.GetAll(new Level4Specs(1, false));
            var level4 = await _unitOfWork.Level4.GetAll(new Level4Specs());
            if (!level4.Any())
                return new Response<List<Level4Dto>>(null, "List is empty");

            return new Response<List<Level4Dto>>(_mapper.Map<List<Level4Dto>>(level4), "Returning List");

        }

        public async Task<Response<List<Level4Dto>>> GetReceivableAccounts()
        {//SBBU-Code
            //var level4 = await _unitOfWork.Level4.GetAll(new Level4Specs(2, true));
            var level4 = await _unitOfWork.Level4.GetAll(new Level4Specs());
            if (!level4.Any())
                return new Response<List<Level4Dto>>(null, "List is empty");

            return new Response<List<Level4Dto>>(_mapper.Map<List<Level4Dto>>(level4), "Returning List");
        }

        public async Task<Response<List<Level4Dto>>> GetNonCurrentAssetAccounts()
        {
            var level4 = await _unitOfWork.Level4.GetAll(new Level4Specs("", ""));
            if (!level4.Any())
                return new Response<List<Level4Dto>>(null,"List is empty");

            return new Response<List<Level4Dto>>(_mapper.Map<List<Level4Dto>>(level4), "Returning List");
        }

        public async Task<Response<List<Level4Dto>>> GetNonCurrentLiabilitiesAccounts()
        {// SBBU-Code
            //var level4 = await _unitOfWork.Level4.GetAll(new Level4Specs("", "" , 1));
            var level4 = await _unitOfWork.Level4.GetAll(new Level4Specs());
            if (!level4.Any())
                return new Response<List<Level4Dto>>(null, "List is empty");

            return new Response<List<Level4Dto>>(_mapper.Map<List<Level4Dto>>(level4), "Returning List");
        }
        public async Task<Response<List<Level4Dto>>> GetExpenseAccounts()
        {
            int tenantId = GetTenant.GetTenantId(_httpContextAccessor);
            var level4 = await _unitOfWork.Level4.GetAll(new GetExpenseAccountsSpecs(tenantId));
            if (!level4.Any())
                return new Response<List<Level4Dto>>(null, "List is empty");

            return new Response<List<Level4Dto>>(_mapper.Map<List<Level4Dto>>(level4), "Returning List");
        }
        public async Task<Response<List<Level4Dto>>> GetIncomeAccounts()
        {
            int tenantId = GetTenant.GetTenantId(_httpContextAccessor);
            var level4 = await _unitOfWork.Level4.GetAll(new GetIncomeAccountsSpecs(tenantId));
            if (!level4.Any())
                return new Response<List<Level4Dto>>(null, "List is empty");

            return new Response<List<Level4Dto>>(_mapper.Map<List<Level4Dto>>(level4), "Returning List");
        }
        public async Task<Response<List<Level4Dto>>> GetCashBankAccounts()
        {
            int tenantId = GetTenant.GetTenantId(_httpContextAccessor);
            var level4 = await _unitOfWork.Level4.GetAll(new GetCashBankAccountsSpecs(tenantId));
            if (!level4.Any())
                return new Response<List<Level4Dto>>(null, "List is empty");

            return new Response<List<Level4Dto>>(_mapper.Map<List<Level4Dto>>(level4), "Returning List");
        }
        public async Task<Response<List<Level4Dto>>> GetCurrentAssetAccounts()
        {
            int tenantId = GetTenant.GetTenantId(_httpContextAccessor);
            var level4 = await _unitOfWork.Level4.GetAll(new GetCurrentAssetAccountsSpecs(tenantId));
            if (!level4.Any())
                return new Response<List<Level4Dto>>(null, "List is empty");

            return new Response<List<Level4Dto>>(_mapper.Map<List<Level4Dto>>(level4), "Returning List");
        }

        public async Task<Response<bool>> DeleteCOAs(List<string> ids)
        {
            if(ids.Count() < 0 || ids == null)
            {
                return new Response<bool>("List Cannot be Empty.");
            }
            else
            {
                foreach (var coa in ids)
                {
                    var account = await _unitOfWork.Level4.GetById(coa);                                    
                    if (account == null)
                        return new Response<bool>("Account Not Found.");
                    account.IsDelete = true;
                    await _unitOfWork.SaveAsync();
                }
                    return new Response<bool>(true, "Deleted Successfully.");
            }                   
        }
    }
}
