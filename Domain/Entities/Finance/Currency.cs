using Domain.Base;
using Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Currency : BaseEntity<int>, IMustHaveTenant
    {
        public string Code { get; private set; }
        public string Symbol { get; private set; }
        public string Name { get; private set; }
        public string Unit { get; private set; }
        public string SubUnit { get; private set; }
        public DateTime? LastUpdate { get; private set; }
        public decimal UnitPerUSD { get; private set; }
        public decimal USDPerUnit { get; private set; }

        public IEnumerable<CurrencyLine> CurrencyLines { get; private set; }
        public int OrganizationId { get; set; }


        public void SetCurrencyRate()
        {
            if (CurrencyLines.Count() == 0)
                return;

            // Get the most recent CurrencyLine
            var latestLine = CurrencyLines
                .OrderByDescending(cl => cl.Date) 
                .FirstOrDefault();

            if (latestLine != null)
            {
                LastUpdate = latestLine.Date;
                UnitPerUSD = latestLine.UnitPerUSD; 
                USDPerUnit = latestLine.USDPerUnit; 
            }
        }
    }
}
