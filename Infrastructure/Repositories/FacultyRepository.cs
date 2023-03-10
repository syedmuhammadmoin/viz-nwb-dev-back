using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;

namespace Infrastructure.Repositories
{
    public class FacultyRepository : GenericRepository<Faculty, int>, IFacultyRepository
    {
        public FacultyRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
