using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IProgramRepository : IGenericRepository<Program, int>
    {
        Task AddProgramSemesters(List<ProgramSemester> list);
        Task<IEnumerable<ProgramSemester>> GetProgramSemesters(int programId);
        Task AddProgramCourses(List<ProgramCourse> list);
        Task RemoveProgramCouses(int programId);
        Task<IEnumerable<ProgramCourse>> GetCourses(int programSemesterId);
        Task AddProgramFees(List<ProgramFees> list);
    }
}
