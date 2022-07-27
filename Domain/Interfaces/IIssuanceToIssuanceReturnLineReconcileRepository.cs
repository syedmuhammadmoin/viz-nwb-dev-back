using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IIssuanceToIssuanceReturnLineReconcileRepository
    {
        Task<IssuanceToIssuanceReturnLineReconcile> Add(IssuanceToIssuanceReturnLineReconcile entity);
        IEnumerable<IssuanceToIssuanceReturnLineReconcile> Find(ISpecification<IssuanceToIssuanceReturnLineReconcile> specification);

    }
}
