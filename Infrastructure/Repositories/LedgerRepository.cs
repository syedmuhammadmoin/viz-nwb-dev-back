using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class LedgerRepository : GenericRepository<RecordLedger, int>, ILedgerRepository
    {
        private readonly ApplicationDbContext _context;
        public LedgerRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddRange(List<RecordLedger> list)
        {
            await _context.RecordLedger.AddRangeAsync(list);
        }
    }
}
