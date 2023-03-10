using Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Faculty : BaseEntity<int>
    {
        [MaxLength(200)]
        public string Name { get; private set; }

        protected Faculty()
        {
        }
    }
}
