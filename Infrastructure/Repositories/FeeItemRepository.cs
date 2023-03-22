using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;

namespace Infrastructure.Repositories
{
    public class FeeItemRepository : GenericRepository<FeeItem, int>, IFeeItemRepository
    {
        public FeeItemRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
