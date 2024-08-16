using Domain.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class ChartOfAccountType : BaseEntity<int>
    {
        [MaxLength(200)]
        public string Name { get; private set; }

        protected ChartOfAccountType()
        {
        }
    }
}
