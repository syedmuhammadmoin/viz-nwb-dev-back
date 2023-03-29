using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace Domain.Entities
{
    public class District : BaseEntity<int>
    {
        [MaxLength(200)]
        public string Name { get; private set; }

        public int CityId { get; private set; }
        [ForeignKey("CityId")]
        public City City { get; private set; }

        protected District()
        {
        }
    }
}
