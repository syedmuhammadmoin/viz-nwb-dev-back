using Application.Contracts.Filters;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class FixedAssetReportSpecs : BaseSpecification<RecordLedger>
    {
        public FixedAssetReportSpecs(FixedAssetReportFilter filters, List<int?> fixedAssets, List<int?> stores) 
            : base(i => i.TransactionDate < filters.FromDate
            && (fixedAssets.Count() > 0 ? fixedAssets.Contains(i.FixedAssetId) : true)
            && (stores.Count() > 0 ? stores.Contains(i.WarehouseId) : true)
            && i.FixedAssetId != null)
        {
            AddInclude(i => i.Warehouse);
            AddInclude("FixedAsset.Product.Category");
        }

        public FixedAssetReportSpecs(FixedAssetReportFilter filters, List<int?> fixedAssets, List<int?> stores, bool forPeriod)
            : base(i => (i.TransactionDate >= filters.FromDate && i.TransactionDate <= filters.ToDate)
            && (fixedAssets.Count() > 0 ? fixedAssets.Contains(i.FixedAssetId) : true)
            && (stores.Count() > 0 ? stores.Contains(i.WarehouseId) : true)
            && i.FixedAssetId != null)
        {
            AddInclude(i => i.Warehouse);
            AddInclude("FixedAsset.Product.Category");
        }
    }
}
