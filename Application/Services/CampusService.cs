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
    public class CampusService : ICampusService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CampusService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<CampusDto>> CreateAsync(CreateCampusDto[] entity)
        {
            List<Campus> campusList = new List<Campus>();
            foreach (var item in entity)
            {
                var getCampus = await _unitOfWork.Campus.GetById((int)item.Id);

                if (getCampus != null)
                {
                    _mapper.Map<CreateCampusDto, Campus>(item, getCampus);
                }
                else
                {
                    campusList.Add(_mapper.Map<Campus>(item));
                }
            }

            await _unitOfWork.SaveAsync();

            if (campusList.Any())
            {
                await _unitOfWork.Campus.AddRange(campusList);
                await _unitOfWork.SaveAsync();
            }

            return new Response<CampusDto>(null, "Records populated successfully");
        }

        public async Task<PaginationResponse<List<CampusDto>>> GetAllAsync(TransactionFormFilter filter)
        {
            var campus = await _unitOfWork.Campus.GetAll(new CampusSpecs(filter, false));

            if (campus.Count() == 0)
                return new PaginationResponse<List<CampusDto>>(_mapper.Map<List<CampusDto>>(campus), "List is empty");

            var totalRecords = await _unitOfWork.Campus.TotalRecord(new CampusSpecs(filter, true));

            return new PaginationResponse<List<CampusDto>>(_mapper.Map<List<CampusDto>>(campus), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<CampusDto>> GetByIdAsync(int id)
        {
            var campus = await _unitOfWork.Campus.GetById(id);
            if (campus == null)
                return new Response<CampusDto>("Not found");

            return new Response<CampusDto>(_mapper.Map<CampusDto>(campus), "Returning value");
        }

        //public async Task<Response<CampusDto>> UpdateAsync(CreateCampusDto[] entity)
        //{
        //    var campus = await _unitOfWork.Campus.GetById((int)entity.Id);

        //    if (campus == null)
        //        return new Response<CampusDto>("Not found");

        //    //For updating data
        //    _mapper.Map<CreateCampusDto, Campus>(entity, campus);
        //    await _unitOfWork.SaveAsync();
        //    return new Response<CampusDto>(_mapper.Map<CampusDto>(campus), "Updated successfully");
        //}

        public async Task<Response<List<CampusDto>>> GetCampusDropDown()
        {
            var campuses = await _unitOfWork.Campus.GetAll();
            if (!campuses.Any())
                return new Response<List<CampusDto>>("List is empty");

            return new Response<List<CampusDto>>(_mapper.Map<List<CampusDto>>(campuses), "Returning List");
        }

        public Task<Response<CampusDto>> UpdateAsync(CreateCampusDto[] entity)
        {
            throw new NotImplementedException();
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
