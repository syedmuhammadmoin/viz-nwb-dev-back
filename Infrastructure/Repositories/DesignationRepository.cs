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
    public class DesignationRepository : GenericRepository<Designation, int>, IDesignationRepository
    {
        private readonly ApplicationDbContext _context;
        public DesignationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task AddRange(List<Designation> list)
        {
            await _context.Designations.AddRangeAsync(list);
        }
    }
}
