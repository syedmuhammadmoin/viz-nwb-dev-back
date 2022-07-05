using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IPOToGRNLineReconcileRepository
    {
        Task<POToGRNLineReconcile> Add(POToGRNLineReconcile entity);

        IEnumerable<POToGRNLineReconcile> Find(ISpecification<POToGRNLineReconcile> specification);
    }
}
