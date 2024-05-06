using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PayrollItemRepository : GenericRepository<PayrollItem, int>, IPayrollItemRepository
    {
        private readonly ApplicationDbContext _context;

        public PayrollItemRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }
        public List<PayrollResult> GetPayrollItemsByEmployeeId(int id)
        {                                           
            //var payrollQuery = from pe in _context.PayrollItems
            //                   join pie in _context.PayrollItemEmployees on pi. equals pie.PayrollItemId
            //                   where pie.EmployeeId == id
            //                   select new PayrollResult
            //                   {                                                                                                                                                
            //                       Id = pie.PayrollItemId,
            //                       ItemCode = pi.ItemCode,
            //                       Name = pi.Name,
            //                       Value = pi.Value 
            //                   };
            var query = from pe in _context.PayrollItemEmployees
                        join pi in _context.PayrollItems on pe.PayrollItemId equals pi.Id
                        where pe.EmployeeId == id
                        select new PayrollResult
                        {
                            Id = pi.Id,
                            ItemCode = pi.ItemCode,
                            Value = pi.Value,
                            Name = pi.Name,
                        };

            return query.ToList();
        }
    }
}
