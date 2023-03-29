using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;

namespace Infrastructure.Repositories
{
    public class StateRepository : GenericRepository<State, int>, IStateRepository
    {
        public StateRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
