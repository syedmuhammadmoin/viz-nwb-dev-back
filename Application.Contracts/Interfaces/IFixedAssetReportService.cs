using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Response;

namespace Application.Contracts.Interfaces
{
    public interface IFixedAssetReportService
    {
        Response<List<FixedAssetReportDto>> GetReport(FixedAssetReportFilter filters);
    }
}
