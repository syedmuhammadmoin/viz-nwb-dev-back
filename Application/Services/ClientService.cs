using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Application.Specifications;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ClientService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<ClientDto>> CreateAsync(CreateClientDto entity)
        {
            var client = new Client(_mapper.Map<Client>(entity));
            var result = await _unitOfWork.Client.Add(client);
            await _unitOfWork.SaveAsync();

            return new Response<ClientDto>(_mapper.Map<ClientDto>(result), "Created successfully");
        }

        public async Task<PaginationResponse<List<ClientDto>>> GetAllAsync(PaginationFilter filter)
        {
            var specification = new ClientSpecs(filter);
            var clients = await _unitOfWork.Client.GetAll(specification);

            if (clients.Count() == 0)
                return new PaginationResponse<List<ClientDto>>("List is empty");

            var totalRecords = await _unitOfWork.Client.TotalRecord();

            return new PaginationResponse<List<ClientDto>>(_mapper.Map<List<ClientDto>>(clients), filter.PageStart, filter.PageEnd, totalRecords, "Returing list");
        }

        public async Task<Response<ClientDto>> GetByIdAsync(int id)
        {
            var client =  await _unitOfWork.Client.GetById(id);
            if (client == null)
                return new Response<ClientDto>("Not found");

            return new Response<ClientDto>(_mapper.Map<ClientDto>(client), "Returning value");
        }

        public async Task<Response<ClientDto>> UpdateAsync(CreateClientDto entity)
        {
            var client = await _unitOfWork.Client.GetById((int)entity.Id);

            if (client == null)
                return new Response<ClientDto>("Not found");

            //For updating data
            _mapper.Map<CreateClientDto, Client>(entity, client);
            await _unitOfWork.SaveAsync();
            return new Response<ClientDto>(_mapper.Map<ClientDto>(client), "Updated successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
