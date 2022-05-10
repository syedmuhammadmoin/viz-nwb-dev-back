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
    public class PayrollItemEmpRepository : IPayrollItemEmpRepository
    {
        private readonly ApplicationDbContext _context;
        public PayrollItemEmpRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddRange(List<PayrollItemEmployee> list)
        {
            await _context.PayrollItemEmployees.AddRangeAsync(list);
        }

        public async Task<List<PayrollItemEmployee>> GetById(int id)
        {
            return await _context.PayrollItemEmployees.Select(x => x.Id).Where(x=> x);
        }
    }
}
