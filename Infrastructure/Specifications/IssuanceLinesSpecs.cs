﻿using Domain.Constants;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public class IssuanceLinesSpecs : BaseSpecification<IssuanceLines>
    {
        public IssuanceLinesSpecs(int itemId, int warehouseId, int masterId)
        : base(x => x.ItemId == itemId && x.WarehouseId == warehouseId && x.MasterId == masterId
        && (x.Status == DocumentStatus.Partial || x.Status == DocumentStatus.Unreconciled))
        {
        }
    }
}
