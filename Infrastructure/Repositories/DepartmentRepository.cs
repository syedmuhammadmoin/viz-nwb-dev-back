using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class DepartmentRepository : GenericRepository<Department, int>, IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;
        public DepartmentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddRange(List<Department> list)
        {
            await _context.Departments.AddRangeAsync(list);
        }
    }
}
