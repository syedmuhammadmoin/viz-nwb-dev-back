using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace Domain.Entities
{
    public class State : BaseEntity<int>
    {
        [MaxLength(200)]
        public string Name { get; private set; }

        public int CountryId { get; private set; }
        [ForeignKey("CountryId")]
        public Country Country { get; private set; }

        protected State()
        {
        }
    }
}
