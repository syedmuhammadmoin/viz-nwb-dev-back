using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IProgramCourseRepository : IGenericRepository<ProgramCourse, int>
    {
        Task AddRange(List<ProgramCourse> list);
        Task<IEnumerable<ProgramCourse>> GetByProgramId(int programId);
    }
}
