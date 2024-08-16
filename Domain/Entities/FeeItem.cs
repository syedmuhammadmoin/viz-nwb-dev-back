using Domain.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class FeeItem : BaseEntity<int>
    {
        [MaxLength(200)]
        public string Name { get; private set; }
        public string AccountId { get; private set; }
        [ForeignKey("AccountId")]
        public Level4 Account { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; private set; }
        protected FeeItem()
        {
        }
    }
}
