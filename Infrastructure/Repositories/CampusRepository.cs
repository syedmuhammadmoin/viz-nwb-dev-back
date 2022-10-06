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
    public class CampusRepository : GenericRepository<Campus, int>, ICampusRepository
    {
        private readonly ApplicationDbContext _context;
        public CampusRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;

        }
        public async Task AddRange(List<Campus> list)
        {
            await _context.Campuses.AddRangeAsync(list);
        }
    }
}
