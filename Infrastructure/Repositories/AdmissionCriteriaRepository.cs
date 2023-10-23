using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;

namespace Infrastructure.Repositories
{
    public class AdmissionCriteriaRepository : GenericRepository<AdmissionCriteria, int>, IAdmissionCriteriaRepository
    {
        public AdmissionCriteriaRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
