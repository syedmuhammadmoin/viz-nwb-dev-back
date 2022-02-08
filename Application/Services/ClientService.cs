using Application.Contracts.DTOs;
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

        public async Task<Response<List<ClientDto>>> GetAllAsync()
        {
            var clients = await _unitOfWork.Client.GetAll();
            return new Response<List<ClientDto>>(_mapper.Map<List<ClientDto>>(clients), "Returing clients list");
        }

        public async Task<Response<ClientDto>> GetByIdAsync(int id)
        {
            var client = await _unitOfWork.Client.GetById(id);
            if (client == null)
                return new Response<ClientDto>("Client not found");

            return new Response<ClientDto>(_mapper.Map<ClientDto>(client), "Returning client");
        }

        public async Task<Response<ClientDto>> CreateAsync(CreateClientDto entity)
        {
            var client = new Client(_mapper.Map<Client>(entity));
            var result = await _unitOfWork.Client.Add(client);
            await _unitOfWork.SaveAsync();

            return new Response<ClientDto>(_mapper.Map<ClientDto>(result), "Client created successfully");
        }

        public async Task<Response<ClientDto>> UpdateAsync(CreateClientDto entity)
        {
            var client = await _unitOfWork.Client.GetById((int)entity.Id);
            
            if (client == null)
                return new Response<ClientDto>("Client not found");

            //For updating data
            _mapper.Map<CreateClientDto, Client>(entity, client);
            await _unitOfWork.SaveAsync();
            return new Response<ClientDto>(_mapper.Map<ClientDto>(client), "Client created successfully");
        }

        public Task<Response<int>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ClientDto> Find()
        {
            var specification = new ClientWithSpecificName("Hamza");
            //var specification = new DeveloperByIncomeSpecification();
            var client = _unitOfWork.Client.Find(specification);
            return _mapper.Map<IEnumerable<ClientDto>>(client);
        }
    }
}
