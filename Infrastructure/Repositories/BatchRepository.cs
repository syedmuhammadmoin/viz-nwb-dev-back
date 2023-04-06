using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;

namespace Infrastructure.Repositories
{
    public class BatchRepository : GenericRepository<BatchMaster, int>, IBatchRepository
    {
        public BatchRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
