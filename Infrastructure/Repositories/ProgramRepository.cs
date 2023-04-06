using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;

namespace Infrastructure.Repositories
{
    public class ProgramRepository : GenericRepository<Program, int>, IProgramRepository
    {
        public ProgramRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
