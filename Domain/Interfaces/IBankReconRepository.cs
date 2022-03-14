﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IBankReconRepository : IGenericRepository<BankReconciliation, int>
    {
        Task<decimal> GetReconciledAmountById(int id, bool isPaymetId);
    }
}