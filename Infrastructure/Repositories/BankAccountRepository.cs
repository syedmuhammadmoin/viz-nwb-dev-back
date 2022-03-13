using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class BankAccountRepository : GenericRepository<BankAccount, int>, IBankAccountRepository
    {
        private readonly ApplicationDbContext _context;
        public BankAccountRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<BankAccount> GetByClearingAccountId(Guid id)
        {
            return await _context.BankAccounts.FirstOrDefaultAsync(e => e.ClearingAccountId == id);
        }
    }
}
