using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IRequisitionToIssuanceLineReconcileRepository
    {
        Task<RequisitionToIssuanceLineReconcile> Add(RequisitionToIssuanceLineReconcile entity);

        IEnumerable<RequisitionToIssuanceLineReconcile> Find(ISpecification<RequisitionToIssuanceLineReconcile> specification);
    }
}
