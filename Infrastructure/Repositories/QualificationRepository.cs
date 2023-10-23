using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;

namespace Infrastructure.Repositories
{
    public class QualificationRepository : GenericRepository<Qualification, int>, IQualificationRepository
    {
        public QualificationRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
