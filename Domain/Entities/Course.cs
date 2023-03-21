using Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Course : BaseEntity<int>
    {
        [MaxLength(100)]
        public string Name { get; private set; }

        protected Course()
        {
        }
    }
}
