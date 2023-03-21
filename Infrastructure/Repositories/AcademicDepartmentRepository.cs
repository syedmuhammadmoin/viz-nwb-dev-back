using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;

namespace Infrastructure.Repositories
{
    public class AcademicDepartmentRepository : GenericRepository<AcademicDepartment, int>, IAcademicDepartmentRepository
    {
        public AcademicDepartmentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
