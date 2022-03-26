using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ITransactionReconcileRepository
    {
        Task<TransactionReconcile> Reconcile(TransactionReconcile entity);

        IEnumerable<TransactionReconcile> Find(ISpecification<TransactionReconcile> specification);
    }
}
