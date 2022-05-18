using Application.Contracts.Response;
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

        public async Task<bool> RemoveAllByPayrollItemId(int id)
        {
           //getting employees via PayrollItem
            var getList = await _context.PayrollItemEmployees
                .Where(e => e.PayrollItemId== id)
                .ToListAsync();
            
           //removing previous employees from payrollItems
            if (getList.Any())
            {
                _context.PayrollItemEmployees.RemoveRange(getList);
                _context.SaveChanges();
                return true;
            }

           
            return true;
        }

        public IEnumerable<PayrollItemEmployee> Find(ISpecification<PayrollItemEmployee> specification)
        {
            return SpecificationEvaluator<PayrollItemEmployee, int>.GetQuery(_context.PayrollItemEmployees
                                    .AsQueryable(), specification);
        }
    }
}
