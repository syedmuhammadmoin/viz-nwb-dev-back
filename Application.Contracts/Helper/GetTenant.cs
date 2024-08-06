using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Helper
{
    public static class GetTenant
    {
        public static int GetTenantId(IHttpContextAccessor httpContextAccessor)
        {
            var httpContext = httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                var orgId = httpContext.User.Claims.FirstOrDefault(i => i.Type == "Organization").Value;
                return Int32.Parse(orgId);
            }
            return 0;
        }
    }
}
