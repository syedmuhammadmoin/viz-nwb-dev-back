using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace Domain.Entities
{
    public class AppPackge : BaseEntity<int>
    {
        [MaxLength(200)]
        public string Name { get; private set; }
        public DateTime? Date { get; private set; }
        public int UserLimit { get; private set; }
        public int OrganizationLimit { get; private set; }
        public decimal MonthlyPrice { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public string DiscountType { get; private set; } // P for % A for Amount
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountOnQuater { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountOnYear { get; private set; }

        protected AppPackge()
        {
        }
    }
}
