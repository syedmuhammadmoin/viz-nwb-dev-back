using Domain.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class ChartOfAccountSubType : BaseEntity<int>
    {
        [MaxLength(200)]
        public string Name { get; private set; }

        public int ChartOfAccountTypeId { get; private set; }
        [ForeignKey("ChartOfAccountTypeId")]
        public ChartOfAccountType ChartOfAccountType { get; private set; }

        protected ChartOfAccountSubType()
        {
        }
    }
}
