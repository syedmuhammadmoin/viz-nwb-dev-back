using Application.Contracts.DTOs;
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
    public class LedgerRepository : GenericRepository<RecordLedger, int>, ILedgerRepository
    {
        private readonly ApplicationDbContext _context;
        public LedgerRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public new async Task<RecordLedger> Add(RecordLedger entity)
        {
            //Getting level 3 account id
            var getLevel3 = await _context.Level4
                .Where(i => i.Id == entity.Level4_id)
                .Select(i => i.Level3_id)
                .FirstOrDefaultAsync();

            // Setting isReconcilable true if account id is equal to payable or receivable
            if (getLevel3 == new Guid("12200000-5566-7788-99AA-BBCCDDEEFF00")
                || getLevel3 == new Guid("12100000-5566-7788-99AA-BBCCDDEEFF00")
                || getLevel3 == new Guid("12300000-5566-7788-99AA-BBCCDDEEFF00")
                || getLevel3 == new Guid("12900000-5566-7788-99AA-BBCCDDEEFF00")
                || getLevel3 == new Guid("12110000-5566-7788-99AA-BBCCDDEEFF00")
                || getLevel3 == new Guid("12120000-5566-7788-99AA-BBCCDDEEFF00")
                || getLevel3 == new Guid("22100000-5566-7788-99AA-BBCCDDEEFF00"))
            {
                entity.SetIsReconcilable(true);
            }
            else
            {
                entity.SetIsReconcilable(false);
            }
            //Adding in recordLedger
            var result = await _context.RecordLedger.AddAsync(entity);
            return result.Entity;
        }

        public async Task AddRange(List<RecordLedger> list)
        {
            foreach (var item in list)
            {
                //Getting level 3 account id
                var getLevel3 = await _context.Level4
                    .Where(i => i.Id == item.Level4_id)
                    .Select(i => i.Level3_id)
                    .FirstOrDefaultAsync();

                // Setting isReconcilable true if account id is equal to payable or receivable
                if (getLevel3 == new Guid("12200000-5566-7788-99AA-BBCCDDEEFF00")
                || getLevel3 == new Guid("12100000-5566-7788-99AA-BBCCDDEEFF00")
                || getLevel3 == new Guid("12300000-5566-7788-99AA-BBCCDDEEFF00")
                || getLevel3 == new Guid("12900000-5566-7788-99AA-BBCCDDEEFF00")
                || getLevel3 == new Guid("12110000-5566-7788-99AA-BBCCDDEEFF00")
                || getLevel3 == new Guid("12120000-5566-7788-99AA-BBCCDDEEFF00")
                || getLevel3 == new Guid("22100000-5566-7788-99AA-BBCCDDEEFF00"))
                {
                    item.SetIsReconcilable(true);
                }
                else
                {
                    item.SetIsReconcilable(false);
                }
            }
            await _context.RecordLedger.AddRangeAsync(list);
        }


        public new IEnumerable<RecordLedger> Find(ISpecification<RecordLedger> specification)
        {
            return SpecificationEvaluator<RecordLedger, int>.GetQuery(_context.RecordLedger
                                    .AsQueryable(), specification).AsNoTracking();
        }
        public IEnumerable<dynamic> GetBankAccountBalanceSummary()
        {

            var result = _context.RecordLedger
                            .Join(_context.Level4, recordLedger => recordLedger.Level4_id, level4 => level4.Id, (rl, l4) => new { rl, l4 })
                            .Join(_context.BankAccounts,
                                 temp => temp.l4.Id,
                                 ba => ba.ChAccountId ,
                                 (temp, ba) => new { temp.rl, temp.l4, ba })
                             .Union(_context.RecordLedger
                                 .Join(_context.Level4, rl => rl.Level4_id, l4 => l4.Id, (rl, l4) => new { rl, l4 })
                                 .Join(_context.BankAccounts,
                                     temp => temp.l4.Id,
                                     ba => ba.ClearingAccountId,
                                     (temp, ba) => new { temp.rl, temp.l4, ba }))
                         .GroupBy(joined => new { joined.ba.BankName, joined.ba.AccountTitle })
                         .Select(grouped => new
                         {
                             BankName = grouped.Key.BankName,
                             AccountTitle = grouped.Key.AccountTitle,
                             ReconcileBalance = grouped.Sum(item => (item.l4.Id == item.ba.ChAccountId) ? (item.rl.Sign == 'D' ? item.rl.Amount : -item.rl.Amount) : 0),
                             UnReconcileBalance = grouped.Sum(item => (item.l4.Id == item.ba.ClearingAccountId) ? (item.rl.Sign == 'D' ? item.rl.Amount : -item.rl.Amount) : 0),
                             Balance = grouped.Sum(item => (item.rl.Sign == 'D' ? item.rl.Amount : -item.rl.Amount))
                         });
            return result.ToList();

        }



    }
}
