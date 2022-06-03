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
            if (getLevel3 == new Guid("12200000-5566-7788-99AA-BBCCDDEEFF00") || getLevel3 == new Guid("22100000-5566-7788-99AA-BBCCDDEEFF00"))
            {
                entity.setIsReconcilable(true);
            }
            else
            {
                entity.setIsReconcilable(false);
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
                if (getLevel3 == new Guid("12200000-5566-7788-99AA-BBCCDDEEFF00") || getLevel3 == new Guid("22100000-5566-7788-99AA-BBCCDDEEFF00"))
                {
                    item.setIsReconcilable(true);
                }
                else
                {
                    item.setIsReconcilable(false);
                }
            }
            await _context.RecordLedger.AddRangeAsync(list);
        }


        public new IEnumerable<RecordLedger> Find(ISpecification<RecordLedger> specification)
        {
            return SpecificationEvaluator<RecordLedger, int>.GetQuery(_context.RecordLedger
                                    .AsQueryable(), specification).AsNoTracking();
        }
    }
}
