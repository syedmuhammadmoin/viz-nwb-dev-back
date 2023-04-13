using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;

namespace Infrastructure.Repositories
{
    public class AdmissionApplicationRepository : GenericRepository<AdmissionApplication, int>, IAdmissionApplicationRepository
    {
        public AdmissionApplicationRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
