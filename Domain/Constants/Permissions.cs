using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Constants
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionsForModule(string module)
        {
            return new List<string>()
            {
                $"Permissions.{module}.Create",
                $"Permissions.{module}.View",
                $"Permissions.{module}.Edit",
                $"Permissions.{module}.Delete",
            };
        }

        public static List<string> GeneratePermissionsForModuleReporting(string module)
        {
            return new List<string>()
            {
                $"Permissions.{module}.View",
            };
        }

        public static class AuthClaims
        {
            public const string View = "Permissions.AuthClaims.View";
            public const string Create = "Permissions.AuthClaims.Create";
            public const string Edit = "Permissions.AuthClaims.Edit";
            public const string Delete = "Permissions.AuthClaims.Delete";
        }
    }
}
