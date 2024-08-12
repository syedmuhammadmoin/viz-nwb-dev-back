using System;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class Level3Repository : GenericRepository<Level3, string>, ILevel3Repository
    {
        private readonly ApplicationDbContext _context;
        public Level3Repository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddRange(List<Level3> list)
        {
            await _context.Level3.AddRangeAsync(list);
        }
    }
}
