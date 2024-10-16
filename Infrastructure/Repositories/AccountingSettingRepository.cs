using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AccountingSettingRepository : GenericRepository<AccountingSettingEntity , int> , IAccountingSettingRepository
    {
        private readonly ApplicationDbContext _options;

        public AccountingSettingRepository(ApplicationDbContext options) : base(options)
        {
            _options = options;
        }
        public void DeleteAll()
        {
            _options.Database.ExecuteSqlRaw("DELETE FROM AccountingSettings");
            _options.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('AccountingSettings', RESEED, 0)");
        }
    }
}
