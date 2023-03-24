using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProgramRepository : GenericRepository<Program, int>, IProgramRepository
    {
        private readonly ApplicationDbContext _context;
        public ProgramRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddProgramSemesters(List<ProgramSemester> list)
        {
           await _context.ProgramSemester.AddRangeAsync(list);
        }

        public async Task<IEnumerable<ProgramSemester>> GetProgramSemesters(int programId)
        {
            return await _context.ProgramSemester
                .Where(i => i.ProgramId == programId)
                .Include(i => i.Semester)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddProgramCourses(List<ProgramCourse> list)
        {
            await _context.ProgramCourses.AddRangeAsync(list);
        }

        public async Task RemoveProgramCouses(int programId)
        {
            var getProgramSemesters = await _context.ProgramSemester
                .Where(i => i.ProgramId == programId)
                .ToListAsync();

            foreach (var item in getProgramSemesters)
            {
                var getProgramCourses = await _context.ProgramCourses
                    .Where(i => i.ProgramSemesterId == item.Id)
                    .ToListAsync();
                _context.ProgramCourses.RemoveRange(getProgramCourses);
                await _context.SaveChangesAsync();
            }

            _context.ProgramSemester.RemoveRange(getProgramSemesters);
        }
        
        public async Task<IEnumerable<ProgramCourse>> GetCourses(int programSemesterId)
        {
            return await _context.ProgramCourses
                .Where(i => i.ProgramSemesterId == programSemesterId)
                .Include(i => i.Course)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddProgramFees(List<ProgramFees> list)
        {
            await _context.ProgramFees.AddRangeAsync(list);
        }

    }
}
