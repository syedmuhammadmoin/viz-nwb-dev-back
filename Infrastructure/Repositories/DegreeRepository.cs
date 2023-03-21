using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;

namespace Infrastructure.Repositories
{
    public class DegreeRepository : GenericRepository<Degree, int>, IDegreeRepository
    {
        public DegreeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
