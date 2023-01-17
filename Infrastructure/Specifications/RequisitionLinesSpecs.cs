using Domain.Constants;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class RequisitionLinesSpecs : BaseSpecification<RequisitionLines>
    {
        public RequisitionLinesSpecs(int itemId,int warehouseId, int masterId)
        : base(x => x.ItemId == itemId && x.MasterId == masterId && x.WarehouseId == warehouseId
        && (x.Status == DocumentStatus.Partial || x.Status == DocumentStatus.Unreconciled ))
        {
        }
        public RequisitionLinesSpecs(int itemId, int masterId ,int warehouseId, bool IsRequisition)
       : base(x => x.ItemId == itemId && x.MasterId == masterId && x.WarehouseId == warehouseId)
        {
        }
    }
}
