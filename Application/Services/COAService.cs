using Application.Contracts.DTOs;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using AutoMapper;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class COAService : ICOAService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public COAService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<List<Level1Dto>>> GetCOA()
        {
            var coa = await _unitOfWork.Level4.GetCOA();
            return new Response<List<Level1Dto>>(_mapper.Map<List<Level1Dto>>(coa), "Returning chart of account");
        }
    }
}
