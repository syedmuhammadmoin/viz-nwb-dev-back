using Application.Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Interfaces
{
    public interface IClientService : ICrudService<CreateClientDto, ClientDto, int>
    {
    }
}
