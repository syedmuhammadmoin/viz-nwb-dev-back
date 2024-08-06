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
    public class Level2Repository : GenericRepository<Level2, Guid>, ILevel2Repository
    {
        
        private readonly ApplicationDbContext _context;
        public Level2Repository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddRange(List<Level2> list)
        {
            await _context.Level2.AddRangeAsync(list);
        }
    }
}
