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
    public class Level3Service : ILevel3Service
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public Level3Service(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Response<List<Level3DropDownDto>>> GetLevel3DropDown()
        {
            var level3 = await _unitOfWork.Level3.GetAll(new Level3Specs());
            if (!level3.Any())
                return new Response<List<Level3DropDownDto>>("List is empty");

            return new Response<List<Level3DropDownDto>>(_mapper.Map<List<Level3DropDownDto>>(level3), "Returning List");
        }
       
    }
}
