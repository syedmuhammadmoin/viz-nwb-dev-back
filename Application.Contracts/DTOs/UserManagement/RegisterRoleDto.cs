using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class RegisterRoleDto
    {
        [Required]
        public string RoleName { get; set; }
        public IList<RegisterRoleClaimsDto> RoleClaims { get; set; }

    }
    public class RegisterRoleClaimsDto
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public bool Selected { get; set; }
    }
}
