using Application.Contracts.Response;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Interfaces
{
    public interface ICrudService<VM, DTO, TKey>
    {
        Task<Response<List<DTO>>> GetAllAsync();
        Task<Response<DTO>> GetByIdAsync(TKey id);
        Task<Response<DTO>> CreateAsync(VM entity);
        Task<Response<DTO>> UpdateAsync(VM entity);
        Task<Response<TKey>> DeleteAsync(TKey id);
        IEnumerable<DTO> Find();
    }
}
