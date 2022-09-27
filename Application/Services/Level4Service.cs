using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Specifications;
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

        public Level4Service(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Response<Level4Dto>> CreateAsync(CreateLevel4Dto entity)
        {
            var level3 = _unitOfWork.Level3.Find(new Level3Specs(entity.Level3_id)).FirstOrDefault();
            if (level3 == null)
            {
                return new Response<Level4Dto>("Invalid Level3 Account");
            }
            var level4 = _mapper.Map<Level4>(entity);
            level4.setLevel1Id(level3.Level2.Level1_id);
            var result = await _unitOfWork.Level4.Add(level4);
            await _unitOfWork.SaveAsync();
            return new Response<Level4Dto>(_mapper.Map<Level4Dto>(result), "Created successfully");
        }

        public async Task<PaginationResponse<List<Level4Dto>>> GetAllAsync(PaginationFilter filter)
        {

            var specification = new Level4Specs(filter);
            var level4 = await _unitOfWork.Level4.GetAll(specification);

            if (level4.Count() == 0)
                return new PaginationResponse<List<Level4Dto>>(_mapper.Map<List<Level4Dto>>(level4), "List is empty");

            var totalRecords = await _unitOfWork.Level4.TotalRecord();

            return new PaginationResponse<List<Level4Dto>>(_mapper.Map<List<Level4Dto>>(level4), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");

        }

        public async Task<Response<Level4Dto>> GetByIdAsync(Guid id)
        {
            var specification = new Level4Specs();
            var level4 = await _unitOfWork.Level4.GetById(id, specification);
            if (level4 == null)
                return new Response<Level4Dto>("Not found");

            return new Response<Level4Dto>(_mapper.Map<Level4Dto>(level4), "Returning value");
        }

        public async Task<Response<Level4Dto>> UpdateAsync(CreateLevel4Dto entity)
        {
            var level4 = await _unitOfWork.Level4.GetById((Guid)entity.Id);

            if (level4 == null)
                return new Response<Level4Dto>("Not found");

            if(level4.AccountType == Domain.Constants.AccountType.SystemDefined)
                return new Response<Level4Dto>("System defined accounts cannot be edited");
            
            //For updating data
            _mapper.Map<CreateLevel4Dto, Level4>(entity, level4);
            await _unitOfWork.SaveAsync();
            return new Response<Level4Dto>(_mapper.Map<Level4Dto>(level4), "Updated successfully");
        }
        public Task<Response<Guid>> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<List<Level4Dto>>> GetLevel4DropDown()
        {
            var level4 = await _unitOfWork.Level4.GetAll();
            if (!level4.Any())
                return new Response<List<Level4Dto>>("List is empty");

            return new Response<List<Level4Dto>>(_mapper.Map<List<Level4Dto>>(level4), "Returning List");

        }

        public async Task<Response<List<Level4Dto>>> GetAllOtherAccounts()
        {
            var level4 = await _unitOfWork.Level4.GetAll(new Level4Specs(""));
            if (!level4.Any())
                return new Response<List<Level4Dto>>("List is empty");

            return new Response<List<Level4Dto>>(_mapper.Map<List<Level4Dto>>(level4), "Returning List");

        }

        public async Task<Response<List<Level4Dto>>> GetBudgetAccounts()
        {
            var level4 = await _unitOfWork.Level4.GetAll(new Level4Specs(true));
            if (!level4.Any())
                return new Response<List<Level4Dto>>("List is empty");

            return new Response<List<Level4Dto>>(_mapper.Map<List<Level4Dto>>(level4), "Returning List");

        }

        public async Task<Response<List<Level4Dto>>> GetPayableAccounts()
        {
            var level4 = await _unitOfWork.Level4.GetAll(new Level4Specs(1, false));
            if (!level4.Any())
                return new Response<List<Level4Dto>>("List is empty");

            return new Response<List<Level4Dto>>(_mapper.Map<List<Level4Dto>>(level4), "Returning List");

        }

        public async Task<Response<List<Level4Dto>>> GetReceivableAccounts()
        {
            var level4 = await _unitOfWork.Level4.GetAll(new Level4Specs(2, true));
            if (!level4.Any())
                return new Response<List<Level4Dto>>("List is empty");

            return new Response<List<Level4Dto>>(_mapper.Map<List<Level4Dto>>(level4), "Returning List");
        }
    }
}
