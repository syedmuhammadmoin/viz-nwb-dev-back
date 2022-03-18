using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Helper
{
    public class ClaimRequirementAttribute : TypeFilterAttribute
    {
        public ClaimRequirementAttribute(string claimType, params string[] claimValue) : base(typeof(ClaimRequirementFilter))
        {
            var claims = new Claim[claimValue.Length];
            for (int i = 0; i < claimValue.Length; i++)
            {
                claims[i] = new Claim(claimType, claimValue[i]);
            }

            this.Arguments = new object[] { claims };
        }

        public class ClaimRequirementFilter : IAuthorizationFilter
        {
            readonly Claim[] _claim;

            public ClaimRequirementFilter(Claim[] claim)
            {
                _claim = claim;
            }

            public void OnAuthorization(AuthorizationFilterContext context)
            {
                bool hasClaim = false;
                foreach (var item in _claim)
                {
                    hasClaim = context.HttpContext.User.Claims.Any(p => p.Type == item.Type && p.Value == item.Value);
                    if (hasClaim)
                        break;
                }

                if (!hasClaim)
                {
                    context.Result = new ForbidResult();
                }
            }
        }
    }
}
