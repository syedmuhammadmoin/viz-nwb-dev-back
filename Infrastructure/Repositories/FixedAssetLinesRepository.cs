using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class FixedAssetLinesRepository : GenericRepository<FixedAssetLines, int>, IFixedAssetLinesRepository
    {
        private readonly ApplicationDbContext _context;
        public FixedAssetLinesRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FixedAssetLines>> GetByMonthAndYear(int fixedAssetId, int month, int year)
        {
            return await _context.FixedAssetLines
                .Where(i => i.MasterId == fixedAssetId
                && i.ActiveDate.Month == month && i.ActiveDate.Year == year)
                .ToListAsync();
        }
    }
}
