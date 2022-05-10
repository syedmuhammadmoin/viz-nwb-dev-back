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
    public class PayrollItem : BaseEntity<int>
    {
        [MaxLength(50)]
        public string ItemCode { get; private set; }
        [MaxLength(100)]
        public string Name { get; private set; }
        public PayrollType PayrollType { get; private set; }
        public CalculationType PayrollItemType { get; private set; }
        [Range(0.00, 500.00, ErrorMessage = "Please enter a positive value")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Value { get; private set; }
        public Guid AccountId { get; private set; }
        [ForeignKey("AccountId")]
        public Level4 Account { get; private set; }
        public bool IsActive { get; private set; }
        [MaxLength(300)]
        public string Remarks { get; private set; }
        protected PayrollItem()
        {

        }
    }
}
