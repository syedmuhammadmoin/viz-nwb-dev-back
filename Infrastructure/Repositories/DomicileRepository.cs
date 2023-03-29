using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;

namespace Infrastructure.Repositories
{
    public class DomicileRepository : GenericRepository<Domicile, int>, IDomicileRepository
    {
        public DomicileRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
