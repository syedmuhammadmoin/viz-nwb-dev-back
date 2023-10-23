using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;

namespace Infrastructure.Repositories
{
    public class ApplicantRepository : GenericRepository<Applicant, int>, IApplicantRepository
    {
        public ApplicantRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
