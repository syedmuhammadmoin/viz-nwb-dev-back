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
    public class GeneralSettingRepository : GenericRepository<GeneralSettingEntity,int> , IGeneralSettingRepository
    {
        private readonly ApplicationDbContext _options;

        public GeneralSettingRepository(ApplicationDbContext options) : base(options)
        {
            _options = options;
        }

        public void DeleteAll()
        {
            _options.Database.ExecuteSqlRaw("DELETE FROM GeneralSettings");
            _options.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('GeneralSettings', RESEED, 0)");
        }
    }
}
