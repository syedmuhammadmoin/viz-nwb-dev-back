using Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Semester : BaseEntity<int>
    {
        [MaxLength(50)]
        public string Name { get; private set; }

        protected Semester()
        {
        }
    }
}
