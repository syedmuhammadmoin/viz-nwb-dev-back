using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using Infrastructure.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PurchaseOrderRepository : GenericRepository<PurchaseOrderMaster, int>, IPurchaseOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public PurchaseOrderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<PurchaseOrderLines> FindLines(ISpecification<PurchaseOrderLines> specification)
        {
            return SpecificationEvaluator<PurchaseOrderLines, int>.GetQuery(_context.PurchaseOrderLines
                                     .AsQueryable(), specification);
        }
    }
}
