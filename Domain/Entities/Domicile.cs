using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace Domain.Entities
{
    public class Domicile : BaseEntity<int>
    {
        [MaxLength(200)]
        public string Name { get; private set; }

        public int DistrictId { get; private set; }
        [ForeignKey("DistrictId")]
        public District District { get; private set; }

        protected Domicile()
        {
        }
    }
}
