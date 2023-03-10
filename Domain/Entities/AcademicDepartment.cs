using Domain.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class AcademicDepartment : BaseEntity<int>
    {
        [MaxLength(200)]
        public string Name { get; private set; }

        public int FacultyId { get; private set; }
        [ForeignKey("FacultyId")]
        public Faculty Faculty { get; private set; }

        protected AcademicDepartment()
        {
        }
    }
}
