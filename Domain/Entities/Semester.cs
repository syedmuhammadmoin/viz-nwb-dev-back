using Domain.Base;
using Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Semester : BaseEntity<int>
    {
        [MaxLength(100)]
        public string Name { get; private set; }
        public Season Season { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public bool IsOpenForEnrollment { get; private set; }
        public bool IsActive { get; private set; }

        protected Semester()
        {
        }
    }
}
