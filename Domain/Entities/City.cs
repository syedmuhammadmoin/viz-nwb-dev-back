using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace Domain.Entities
{
    public class City : BaseEntity<int>
    {
        [MaxLength(200)]
        public string Name { get; private set; }

        public int StateId { get; private set; }
        [ForeignKey("StateId")]
        public State State { get; private set; }

        protected City()
        {
        }
    }
}
