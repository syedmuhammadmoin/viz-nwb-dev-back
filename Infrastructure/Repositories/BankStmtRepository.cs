using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class BankStmtRepository : GenericRepository<BankStmtMaster, int>, IBankStmtRepository
    {
        public BankStmtRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
