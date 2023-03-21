using Application.Contracts.DTOs;
using Application.Contracts.Filters;
using Application.Contracts.Interfaces;
using Application.Contracts.Response;
using Domain.Interfaces;
using Infrastructure.Specifications;

namespace Application.Services
{
    public class FixedAssetReportService : IFixedAssetReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        public FixedAssetReportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Response<List<FixedAssetReportDto>> GetReport(FixedAssetReportFilter filters)
        {
            var stores = new List<int?>();
            var fixedAssets = new List<int?>();

            if (filters.ToDate < filters.FromDate)
                return new Response<List<FixedAssetReportDto>>("Start date is greater than end date");


            if (filters.StoreId != null)
            {
                stores.Add(filters.StoreId);
            }

            if (filters.FixedAssetId != null)
            {
                fixedAssets.Add(filters.FixedAssetId);
            }

            //Calling general ledger view
            var forOpeningDepAmount = _unitOfWork.Ledger.Find(new FixedAssetReportSpecs(filters, fixedAssets, stores))
                .Select(i => new FixedAssetReportDto()
                {
                    FixedAssetId = i.FixedAssetId,
                    FixedAsset = i.FixedAsset != null ? i.FixedAsset.Name : "N/A",
                    StoreId = i.WarehouseId,
                    Store = i.Warehouse != null ? i.Warehouse.Name : "N/A",
                    Category = i.FixedAsset != null ? i.FixedAsset.Product.Category.Name : "N/A",
                    UsefullLife = i.FixedAsset.UseFullLife,
                    UnitCost = i.FixedAsset.Cost,
                    DepRate = 0,
                    OpeningAccDep = i.Sign == 'C' ? i.Amount : (-1) * i.Amount,
                    DepChargedForThePeriod = 0,
                    ClosingAccDep = 0,
                    NBV = 0,
                })
                .GroupBy(g => new
                {
                    g.FixedAssetId,
                    g.FixedAsset,
                    g.StoreId,
                    g.Store,
                    g.Category,
                    g.UsefullLife,
                    g.UnitCost,
                    g.DepRate,
                }).Select(i => new FixedAssetReportDto
                {
                    FixedAssetId = i.Key.FixedAssetId,
                    FixedAsset = i.Key.FixedAsset,
                    StoreId = i.Key.StoreId,
                    Store = i.Key.Store,
                    Category = i.Key.Category,
                    UsefullLife = i.Key.UsefullLife,
                    UnitCost = i.Key.UnitCost,
                    DepRate = i.Key.DepRate,
                    OpeningAccDep = i.Sum(s => s.OpeningAccDep),
                    DepChargedForThePeriod = 0,
                    ClosingAccDep = 0,
                    NBV = 0,
                }).ToList();

            var forThePeriodDepAmount = _unitOfWork.Ledger.Find(new FixedAssetReportSpecs(filters, fixedAssets, stores, true))
               .Select(i => new FixedAssetReportDto()
               {
                   FixedAssetId = i.FixedAssetId,
                   FixedAsset = i.FixedAsset != null ? i.FixedAsset.Name : "N/A",
                   StoreId = i.WarehouseId,
                   Store = i.Warehouse != null ? i.Warehouse.Name : "N/A",
                   Category = i.FixedAsset != null ? i.FixedAsset.Product.Category.Name : "N/A",
                   UsefullLife = i.FixedAsset.UseFullLife,
                   UnitCost = i.FixedAsset.Cost,
                   DepRate = 0,
                   OpeningAccDep = 0,
                   DepChargedForThePeriod = i.Sign == 'C' ? i.Amount : (-1) * i.Amount,
                   ClosingAccDep = i.Sign == 'C' ? i.Amount : (-1) * i.Amount,
                   NBV = 0,
               }).GroupBy(g => new
               {
                   g.FixedAssetId,
                   g.FixedAsset,
                   g.StoreId,
                   g.Store,
                   g.Category,
                   g.UsefullLife,
                   g.UnitCost,
                   g.DepRate,
               }).Select(i => new FixedAssetReportDto
               {
                   FixedAssetId = i.Key.FixedAssetId,
                   FixedAsset = i.Key.FixedAsset,
                   StoreId = i.Key.StoreId,
                   Store = i.Key.Store,
                   Category = i.Key.Category,
                   UsefullLife = i.Key.UsefullLife,
                   UnitCost = i.Key.UnitCost,
                   DepRate = i.Key.DepRate,
                   OpeningAccDep = 0,
                   DepChargedForThePeriod = i.Sum(s => s.DepChargedForThePeriod),
                   ClosingAccDep = 0,
                   NBV = 0,
               }).ToList();

            //Applying Union
            var result = forOpeningDepAmount
                .Union(forThePeriodDepAmount)
                .GroupBy(g => new
                {
                    g.FixedAssetId,
                    g.FixedAsset,
                    g.StoreId,
                    g.Store,
                    g.Category,
                    g.UsefullLife,
                    g.UnitCost,
                    g.DepRate,
                }).Select(i => new FixedAssetReportDto
                {
                    FixedAssetId = i.Key.FixedAssetId,
                    FixedAsset = i.Key.FixedAsset,
                    StoreId = i.Key.StoreId,
                    Store = i.Key.Store,
                    Category = i.Key.Category,
                    UsefullLife = i.Key.UsefullLife,
                    UnitCost = i.Key.UnitCost,
                    DepRate = i.Key.UsefullLife == null
                    ? i.Key.UsefullLife == 0 ? 0 : Math.Round((decimal)(100 / (int)i.Key.UsefullLife), 2)
                    : 0,
                    OpeningAccDep = i.Sum(s => s.OpeningAccDep),
                    DepChargedForThePeriod = i.Sum(s => s.DepChargedForThePeriod),
                    ClosingAccDep = i.Sum(s => s.ClosingAccDep),
                    NBV = i.Key.UnitCost - i.Sum(s => s.ClosingAccDep),
                }).ToList();

            return new Response<List<FixedAssetReportDto>>(result, "Returning list");
        }
    }
}
