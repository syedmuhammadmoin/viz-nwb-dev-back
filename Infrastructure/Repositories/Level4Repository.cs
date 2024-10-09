using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;
using Application.Contracts.Helper;

namespace Infrastructure.Repositories
{
    public class Level4Repository : GenericRepository<Level4, string>, ILevel4Repository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Level4Repository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public new async Task<Level4> Add(Level4 entity)
        {
            entity.Id = System.Guid.NewGuid().ToString() + $"-{GetTenant.GetTenantId(_httpContextAccessor)}";

            var getLevel1Id = await _context.Level3
                .Include(c => c.Level2)
                .AsSplitQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == entity.Level3_id);

            if (getLevel1Id == null)
                throw new NullReferenceException("Level 3 account is null");

            entity.Level1_id = getLevel1Id.Level2.Level1_id;
            var result = await _context.Level4.AddAsync(entity);
            return result.Entity;
        }

        public async Task AddRange(List<Level4> list)
        {
            await _context.Level4.AddRangeAsync(list);
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
                    Level2 = x.Level2.Select(y => new Level2
                    {
                        Id = y.Id,
                        Name = y.Name,
                        Level3 = y.Level3.Select(z => new Level3
                        {
                            Id = z.Id,
                            Name = z.Name,
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
        public IQueryable<Level1> GetAccoutTypes()
        {
            return  _context.Level1
                .Include(c => c.Level2)
                .ThenInclude(x => x.Level3)
                .ThenInclude(x => x.Level4);
                
                
        }
    }
}
