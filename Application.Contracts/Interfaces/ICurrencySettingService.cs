using Application.Contracts.DTOs.CurrencySetting;
using Application.Contracts.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Interfaces
{
    public interface ICurrencySettingService : ICrudService<CreateDefaultCurrencySettingDto,DefaultCurrencyDto , int , TransactionFormFilter>
    {
    }
}
