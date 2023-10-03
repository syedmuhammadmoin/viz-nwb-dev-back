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
    public class PayrollTransactionRepository : GenericRepository <PayrollTransactionMaster, int>, IPayrollTransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public PayrollTransactionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
