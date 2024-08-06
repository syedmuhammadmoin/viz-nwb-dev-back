using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.DTOs.Organizations
{
    public class UsersOrganizationDto
    {
        public string UserId { get; private set; }
        public string UserName { get; private set; }
        public int OrganizationId { get; private set; }
        public string OrganizationName { get; private set; }
    }
}
