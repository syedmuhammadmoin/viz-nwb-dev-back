using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Interfaces
{
    public interface IFixedAssetService : ICrudService<CreateFixedAssetDto, FixedAssetDto, int, TransactionFormFilter>
    {
        Task<Response<bool>> CheckWorkFlow(ApprovalDto data);
        Task<Response<List<FixedAssetDto>>> GetAssetDropDown();
        Task<Response<List<FixedAssetDto>>> GetDisposableAssetDropDown();
        Task<Response<List<FixedAssetDto>>> GetAssetByProductIdDropDown(int ProductId);

        

    }
}
