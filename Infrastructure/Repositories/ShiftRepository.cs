using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;

namespace Infrastructure.Repositories
{
    public class ShiftRepository : GenericRepository<Shift, int>, IShiftRepository
    {
        public ShiftRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
