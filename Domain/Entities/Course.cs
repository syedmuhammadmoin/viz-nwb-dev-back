using Domain.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Course : BaseEntity<int>
    {
        [MaxLength(100)]
        public string Name { get; private set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PassingMarks { get; private set; }
        protected Course()
        {
        }
    }
}
