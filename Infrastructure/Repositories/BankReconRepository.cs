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
        public BankReconRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
