using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class Level4Repository : GenericRepository<Level4, Guid>, ILevel4Repository
    {
        private readonly ApplicationDbContext _context;

        public Level4Repository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Level1>> GetCOA()
        {
            return await _context.Level1
                .Include(c => c.Level2)
                .ThenInclude(x => x.Level3)
                .ThenInclude(x => x.Level4)
                .Select(x => new Level1
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    Level2 = x.Level2.Select(y => new Level2
                    {
                        Id = y.Id,
                        Name = y.Name,
                        Code = y.Code,
                        Level3 = y.Level3.Select(z => new Level3
                        {
                            Id = z.Id,
                            Name = z.Name,
                            Code = z.Code,
                            Level4 = z.Level4.Select(a => new Level4
                            {
                                Id = a.Id,
                                Name = a.Name,
                                AccountType = a.AccountType,
                                Code = a.Code
                            })
                        })
                    })
                })
                .ToListAsync();
        }
    }
}
