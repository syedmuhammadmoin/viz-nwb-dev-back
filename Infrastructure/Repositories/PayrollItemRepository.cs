﻿using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PayrollItemRepository : GenericRepository<PayrollItem, int>, IPayrollItemRepository
    {
        public PayrollItemRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
