using Application.Contracts.DTOs.AccountSetting;
using Application.Contracts.Filters;
using Application.Contracts.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Interfaces
{
    public interface IAccountingSettingService : ICrudService<CreateAccountingSettingDto , AccountingSettingDto , int , TransactionFormFilter>
    {    
    }
}
