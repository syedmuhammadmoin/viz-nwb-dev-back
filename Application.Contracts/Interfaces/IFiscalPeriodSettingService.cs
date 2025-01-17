﻿using Application.Contracts.DTOs.FiscalPeriodSetting;
using Application.Contracts.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Interfaces
{
    public interface IFiscalPeriodSettingService : ICrudService<CreateFiscalPeriodSettingDto,FiscalPeriodSettingDto,int ,TransactionFormFilter>
    {
    }
}
