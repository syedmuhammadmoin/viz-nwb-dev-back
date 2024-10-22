﻿using Domain.Entities;
using Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CurrencySettingRepository : GenericRepository<DefaultCurrencySetting,int> , ICurrencySettingRepository
    {
        public CurrencySettingRepository(ApplicationDbContext options):base(options)
        {
            
        }
    }
}
