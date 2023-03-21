using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Base;

namespace Domain.Entities
{
    public class Program : BaseEntity<int>
    {
        [MaxLength(200)]
        public string Name { get; private set; }

        public int DegreeId { get; private set; }
        [ForeignKey("DegreeId")]
        public Degree Degree { get; private set; }

        public int AcademicDepartmentId { get; private set; }
        [ForeignKey("AcademicDepartmentId")]
        public AcademicDepartment AcademicDepartment { get; private set; }

        protected Program()
        {
        }
    }
}
