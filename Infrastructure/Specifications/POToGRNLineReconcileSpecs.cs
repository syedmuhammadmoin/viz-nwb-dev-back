using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class POToGRNLineReconcileSpecs : BaseSpecification<POToGRNLineReconcile>
    {
        public POToGRNLineReconcileSpecs(int purchaseOrderLineId) : base(x => x.PurchaseOrderLineId == purchaseOrderLineId)
        {
        }

        public POToGRNLineReconcileSpecs(bool isPO,int id ) 
            : base(x => isPO ? x.PurchaseOrderId == id : x.GRNId == id)
        {

        }
    }
}
