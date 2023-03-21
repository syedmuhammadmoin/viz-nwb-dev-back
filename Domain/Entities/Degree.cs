using Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities 
{
    public class Degree : BaseEntity<int>
    {
        [MaxLength(100)]
        public string Name { get; private set; }
        protected Degree()
        {
        }
    }
}
