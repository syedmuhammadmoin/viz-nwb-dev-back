using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IRequisitionRepository : IGenericRepository<RequisitionMaster, int>
    {
        IEnumerable<RequisitionLines> FindLines(ISpecification<RequisitionLines> specification);
        dynamic SummarizedbyStatus();
    }
}
