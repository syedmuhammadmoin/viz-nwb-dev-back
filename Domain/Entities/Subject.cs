using Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Subject : BaseEntity<int>
    {
        [MaxLength(50)]
        public string Name { get; private set; }

        protected Subject()
        {
        }
    }
}
