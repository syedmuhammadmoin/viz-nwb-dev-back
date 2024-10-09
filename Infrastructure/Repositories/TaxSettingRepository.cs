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
    public class TaxSettingRepository : GenericRepository<TaxSetting,int>,ITaxSettingRepository
    {
        private readonly ApplicationDbContext _options;

        public TaxSettingRepository(ApplicationDbContext options):base(options)
        {
            _options = options;
        }

        public void DeleteAll()
        {
            _options.Database.ExecuteSqlRaw("DELETE FROM TaxSetting");
            _options.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('TaxSetting', RESEED, 0)");
        }
    }
}
