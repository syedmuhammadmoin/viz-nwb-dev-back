using Domain.Base;
using Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Taxes : BaseEntity<int>
    {
        [MaxLength(80)]
        public string Name { get; private set; }
        public TaxType TaxType { get; private set; }
        public string? AccountId { get; private set; }
        [ForeignKey("AccountId")]
        public Level4 Account { get; private set; }

        public Taxes(int id, string name, TaxType taxType)
        {
            Id = id;
            Name = name;
            TaxType = taxType;
        }

        protected Taxes()
        {

        }
    }
}
