using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs
{
    public class AuthenticationDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public List<string> Permissions { get; set; }
        public int OrganizationId { get; set; }
        public string DateFormat { get; set; }
        public string Currency { get; set; }
        [JsonIgnore]
        public string SAASToken { get; set; }
        public string Token { get; set; }
        public DateTime TokenExpiration { get; set; }
        [JsonIgnore]
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
