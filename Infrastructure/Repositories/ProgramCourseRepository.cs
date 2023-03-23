using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProgramCourseRepository : GenericRepository<ProgramCourse, int>, IProgramCourseRepository
    {
        private readonly ApplicationDbContext _context;
        public ProgramCourseRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddRange(List<ProgramCourse> list)
        {
            await _context.ProgramCourses.AddRangeAsync(list);
        }

        public async Task<IEnumerable<ProgramCourse>> GetByProgramId(int programId)
        {
            return await _context.ProgramCourses
                .Where(i => i.ProgramId == programId)
                .Include(i => i.Semester)
                .Include(i => i.Course)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
