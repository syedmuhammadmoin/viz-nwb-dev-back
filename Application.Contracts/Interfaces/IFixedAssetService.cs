using Application.Contracts.DTOs;
using Application.Contracts.DTOs.FixedAsset;
using Application.Contracts.Filters;
using Application.Contracts.Response;
using Domain.Entities;

namespace Application.Contracts.Interfaces
{
    public interface IFixedAssetService : ICrudService<CreateFixedAssetDto, UpdateFixedAssetDto, FixedAssetDto, int, TransactionFormFilter>
    {
        Task<Response<bool>> CheckWorkFlow(ApprovalDto data);
        Task<Response<List<FixedAssetDto>>> GetDropDown();
        Task<Response<List<FixedAssetDto>>> GetDisposableAssetDropDown();
        Task<Response<List<FixedAssetDto>>> GetAssetInStockByProductIdDropDown(int ProductId);
        Task<Response<bool>> HeldAssetForDisposal(CreateHeldAssetForDisposal createHeldAssetForDisposal);
        Task<Response<FixedAssetDto>> UpdateAfterApproval(UpdateSalvageValueDto entity);
        Task<Response<FixedAssetDto>> DepreciationSchedule(int fixedAssetId);
    }
}
