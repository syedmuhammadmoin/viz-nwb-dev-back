using Application.Contracts.DTOs.GeneralSetting;
using Application.Contracts.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Interfaces
{
    public interface IGeneralSettingService : ICrudService<CreateGeneralSettingDto,GeneralSettingDto,int,TransactionFormFilter>
    {
    }
}
