using Domain.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Subject : BaseEntity<int>
    {
        [MaxLength(50)]
        public string Name { get; private set; }
        public int QualificationId { get; private set; }
        [ForeignKey("QualificationId")]
        public Qualification Qualification { get; private set; }

        protected Subject()
        {
        }
    }
}
