using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Helper
{
    public class GetUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GetUser(IHttpContextAccessor httpContextAccessor)
        {
            this._httpContextAccessor = httpContextAccessor;
        }
        public string GetCurrentUser()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                var authenticatedUserName = httpContext.User.Identity.Name;
                return authenticatedUserName;
            }
            return "UserName";
        }
        public string GetCurrentUserId()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                return userId;
            }
            return "UserName";
        }

        public string GetCurrentUserId()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                return userId;
            }
            return "UserName";
        }
        public IEnumerable<string> GetCurrentUserRoles()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            //Getting user roles
            IEnumerable<string> roles = ((ClaimsIdentity)httpContext.User.Identity).Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value);
            return roles;
        }
    }
}
