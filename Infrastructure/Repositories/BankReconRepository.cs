using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class BankReconRepository : GenericRepository<BankReconciliation, int>, IBankReconRepository
    {
        private readonly ApplicationDbContext _context;
        public BankReconRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<decimal> GetReconciledAmountById(int id, bool isPaymetId)
        {
            if (isPaymetId)
            {
                return await _context.BankReconciliations
                                    .Where(p => p.PaymentId == id)
                                    .SumAsync(p => p.Amount);
            }
            else
            {
                return await _context.BankReconciliations
                                    .Where(p => p.BankStmtId == id)
                                    .SumAsync(p => p.Amount);
            }
        }
    }
}
