using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace Domain.Entities
{
    public class AppUnit : BaseEntity<int>
    {
        [MaxLength(200)]
        public string Name { get; private set; }
        
        [MaxLength(1000)]
        public string Description { get; private set; }

        protected AppUnit()
        {
        }
    }
}
