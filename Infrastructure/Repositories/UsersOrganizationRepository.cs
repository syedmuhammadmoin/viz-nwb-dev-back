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
    public class UsersOrganizationRepository : GenericRepository<UsersOrganization, int>, IUsersOrganization
    {
        private readonly ApplicationDbContext _context;
        public UsersOrganizationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task AddRange(List<UsersOrganization> list)
        {
            await _context.UsersOrganization.AddRangeAsync(list);
        }
    }
}
