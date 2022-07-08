using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IIssuanceRepository : IGenericRepository<IssuanceMaster, int>
    {
        IEnumerable<IssuanceLines> FindLines(ISpecification<IssuanceLines> specification);
    }
}
