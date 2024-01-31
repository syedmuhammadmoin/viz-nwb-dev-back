using Domain.Constants;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class RequisitionRepository : GenericRepository<RequisitionMaster, int>, IRequisitionRepository
    {
        private readonly ApplicationDbContext _context;
        public RequisitionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<RequisitionLines> FindLines(ISpecification<RequisitionLines> specification)
        {
            return SpecificationEvaluator<RequisitionLines, int>.GetQuery(_context.RequisitionLines
                                    .AsQueryable(), specification);
        }


        public dynamic SummarizedbyStatus()
        {
            var statusCounts = _context.RequisitionMaster
                .GroupBy(r => r.StatusId)
                    .Select(g => new
                    {
                        StatusId = g.Key,
                        Count = g.Count()
                    })
                    .ToList();

            foreach (var statusCount in statusCounts)
            {
                DocumentStatus enumStatus = (DocumentStatus)statusCount.StatusId;
                string statusName = enumStatus switch
                {
                    DocumentStatus.Unpaid => "Open",
                    DocumentStatus.Partial => "Open",
                    DocumentStatus.Paid => "Closed",
                    _ => enumStatus.ToString()  // Use the enum value as the default
                };

               
            }
            return statusCounts;

        }
    }
}
