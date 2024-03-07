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
        //public List<dynamic> GetAllPayrollItems(int id)
        //{
        //    var payrollQuery = from pi in _context.PayrollItems
        //                       join pie in _context.PayrollItemEmployees on pi.Id equals pie.PayrollItemId
        //                       where pie.EmployeeId == id
        //                       select new
        //                       {
        //                           pi.ItemCode,
        //                           pi.Name,
        //                           pi.Value
        //                       };

        //    return payrollQuery.ToList();
        //}
    }
}
