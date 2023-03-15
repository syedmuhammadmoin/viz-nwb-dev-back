using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class DepreciationRegisterRepository : GenericRepository<DepreciationRegister, int>, IDepreciationRegisterRepository
    {
        private readonly ApplicationDbContext _context;
        public DepreciationRegisterRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<DepreciationRegister>> GetByMonthAndYear(int fixedAssetId, int month, int year)
        {
            return await _context.DepreciationRegister
                .Where(i => i.FixedAssetId == fixedAssetId
                && i.TransactionDate.Month == month && i.TransactionDate.Year == year)
                .ToListAsync();
        }
    }
}
