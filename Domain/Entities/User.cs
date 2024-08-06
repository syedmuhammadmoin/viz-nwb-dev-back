using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User : IdentityUser
    {
        [MaxLength(100)]
        public string FirstName { get; set; }
        [MaxLength(20)]
        public string DateFormat { get; set; }
        [MaxLength(50)]
        public string Timezone { get; set; }
        public int? EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
        public int? ApplicantId { get; set; }
        [ForeignKey("ApplicantId")]
        public Applicant Applicant { get; set; }
        public int? LastOrganizationAccess { get; set; }
        public ICollection<Organization> Organizations { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }


        public User(int employeeId)
        {
            EmployeeId = employeeId;
        }
        public User()
        {
        }
    }
}
