using Domain.Constants;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Seeds
{
    public static class DefaultUsers
    {
        public static async Task SeedSuperAdminAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            var defaultUser = new User
            {
                FirstName = "Naveed",
                LastName = "Karim",
                UserName = "Naveed",
                Email = "superadmin@vizalys.com",
                EmailConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Admin123!@#");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.SuperAdmin.ToString());
                }
                await roleManager.SeedClaimsForSuperAdmin();
            }
        }
        private async static Task SeedClaimsForSuperAdmin(this RoleManager<IdentityRole> roleManager)
        {
            var superAdmin = await roleManager.FindByNameAsync("SuperAdmin");
            await roleManager.AddPermissionClaim(superAdmin, "AuthClaims");
            await roleManager.AddPermissionClaim(superAdmin, "BusinessPartnerClaims");
            await roleManager.AddPermissionClaim(superAdmin, "OrganizationClaims");
            await roleManager.AddPermissionClaim(superAdmin, "DepartmentClaims");
            await roleManager.AddPermissionClaim(superAdmin, "DesignationClaims");
            await roleManager.AddPermissionClaim(superAdmin, "CampusClaims");
            await roleManager.AddPermissionClaim(superAdmin, "WarehouseClaims");
            await roleManager.AddPermissionClaim(superAdmin, "LocationClaims");
            await roleManager.AddPermissionClaim(superAdmin, "BankAccountClaims");
            await roleManager.AddPermissionClaim(superAdmin, "BankStatementClaims");
            await roleManager.AddPermissionClaim(superAdmin, "CashAccountClaims");
            await roleManager.AddPermissionClaim(superAdmin, "CampusClaims");
            await roleManager.AddPermissionClaim(superAdmin, "CategoriesClaims");
            await roleManager.AddPermissionClaim(superAdmin, "ProductsClaims");
            await roleManager.AddPermissionClaim(superAdmin, "WorkflowStatusClaims");
            await roleManager.AddPermissionClaim(superAdmin, "WorkflowClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Level4Claims");
            await roleManager.AddPermissionClaim(superAdmin, "BankReconClaims");
            await roleManager.AddPermissionClaim(superAdmin, "TransactionReconClaims");
            await roleManager.AddPermissionClaim(superAdmin, "InvoiceClaims");
            await roleManager.AddPermissionClaim(superAdmin, "BillClaims");
            await roleManager.AddPermissionClaim(superAdmin, "PaymentClaims");
            await roleManager.AddPermissionClaim(superAdmin, "CreditNoteClaims");
            await roleManager.AddPermissionClaim(superAdmin, "DebitNoteClaims");
            await roleManager.AddPermissionClaim(superAdmin, "JournalEntryClaims");
            await roleManager.AddPermissionClaim(superAdmin, "BudgetClaims");
            await roleManager.AddPermissionClaim(superAdmin, "ReceiptClaims");
            await roleManager.AddPermissionClaim(superAdmin, "RequisitionClaims");
            await roleManager.AddPermissionClaim(superAdmin, "PurchaseOrderClaims");
            await roleManager.AddPermissionClaim(superAdmin, "GRNClaims");
            await roleManager.AddPermissionClaim(superAdmin, "EstimatedBudgetClaims");
            await roleManager.AddPermissionClaim(superAdmin, "EmployeeClaims");
            await roleManager.AddPermissionClaim(superAdmin, "PayrollItemClaims");
            await roleManager.AddPermissionClaim(superAdmin, "PayrollItemEmployeeClaims");
            await roleManager.AddPermissionClaim(superAdmin, "PayrollTransactionClaims");
            await roleManager.AddPermissionClaim(superAdmin, "PayrollPaymentClaims");
            await roleManager.AddPermissionClaim(superAdmin, "TaxesClaims");
            await roleManager.AddPermissionClaim(superAdmin, "UnitOfMeasurementClaims");
            await roleManager.AddPermissionClaim(superAdmin, "IssuanceClaims");
            await roleManager.AddPermissionClaimReport(superAdmin, "ChartOfAccountClaims");
            await roleManager.AddPermissionClaimReport(superAdmin, "GeneralLedgerClaims");
            await roleManager.AddPermissionClaimReport(superAdmin, "TrialBalanceClaims");
            await roleManager.AddPermissionClaimReport(superAdmin, "ChartOfAccountClaims");
            await roleManager.AddPermissionClaimReport(superAdmin, "BalanceSheetClaims");
            await roleManager.AddPermissionClaimReport(superAdmin, "ProfitLossClaims");
            await roleManager.AddPermissionClaimReport(superAdmin, "BudgetReportClaims");
        }
        public static async Task AddPermissionClaim(this RoleManager<IdentityRole> roleManager, IdentityRole role, string module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = Permissions.GeneratePermissionsForModule(module);

            foreach (var permission in allPermissions)
            {
                if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
                {
                    await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                }
            }
        }
        public static async Task AddPermissionClaimReport(this RoleManager<IdentityRole> roleManager, IdentityRole role, string module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = Permissions.GeneratePermissionsForModuleReporting(module);
            foreach (var permission in allPermissions)
            {
                if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
                {
                    await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                }
            }
        }
    }
}
