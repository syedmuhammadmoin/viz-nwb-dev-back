using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BatchRepository : GenericRepository<BatchMaster, int>, IBatchRepository
    {
        private readonly ApplicationDbContext _context;
        
        public BatchRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddCriteriaInBatch(List<BatchAdmissionCriteria> entity)
        {
            await _context.BatchAdmissionCriteria.AddRangeAsync(entity);
        }

        public async Task RemoveCriteriaFromBatch(int batchId)
        {
            var getBatchCriteria = await _context.BatchAdmissionCriteria
               .Where(i => i.BatchId == batchId)
               .ToListAsync();
            if (getBatchCriteria.Any())
            {
                _context.BatchAdmissionCriteria.RemoveRange(getBatchCriteria);
            }
        }
    }
}
