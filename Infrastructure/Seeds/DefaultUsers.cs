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
        public static async Task SeedSuperAdminAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, string password)
        {
            var defaultUser = new User
            {
                UserName = "Naveed",
                Email = "superadmin@vizalys.com",
                EmailConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, password);
                    await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.SuperAdmin.ToString());
                }
                await roleManager.SeedClaimsForSuperAdmin();
            }
        }
        private async static Task SeedClaimsForSuperAdmin(this RoleManager<IdentityRole> roleManager)
        {
            var superAdmin = await roleManager.FindByNameAsync("SuperAdmin");
            await roleManager.AddPermissionClaim(superAdmin, "AccessManagement", "AuthClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Profiling", "BusinessPartnerClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Profiling", "OrganizationClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Payroll", "DepartmentClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Payroll", "DesignationClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Profiling", "CampusClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Profiling", "WarehouseClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Profiling", "LocationClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Finance", "BankAccountClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Finance", "BankStatementClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Finance", "CashAccountClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Profiling", "CampusClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Profiling", "CategoriesClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Profiling", "ProductsClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Workflow", "WorkflowStatusClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Workflow", "WorkflowClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Finance", "Level4Claims");
            await roleManager.AddPermissionClaim(superAdmin, "Finance", "BankReconClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Finance", "TransactionReconClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Finance", "InvoiceClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Finance", "BillClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Finance", "PaymentClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Finance", "CreditNoteClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Finance", "DebitNoteClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Finance", "JournalEntryClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Budget", "BudgetClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Finance", "ReceiptClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Procurement", "RequisitionClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Procurement", "PurchaseOrderClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Procurement", "GRNClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Budget", "EstimatedBudgetClaims");
            await roleManager.AddPermissionClaim(superAdmin,"Payroll", "EmployeeClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Payroll", "PayrollItemClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Payroll", "PayrollItemEmployeeClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Payroll", "PayrollTransactionClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Payroll", "PayrollPaymentClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Profiling", "TaxesClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Profiling", "UnitOfMeasurementClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Procurement", "IssuanceClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Procurement", "GoodsReturnNoteClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Procurement", "IssuanceReturnClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Procurement", "RequestClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Procurement", "BidEvaluationClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Procurement", "QuotationClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Procurement", "CallForQuotationClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Procurement", "QuotationComparativeClaims");
            await roleManager.AddPermissionClaim(superAdmin, "FixedAsset", "FixedAssetClaims");
            await roleManager.AddPermissionClaim(superAdmin, "FixedAsset", "DepreciationModelClaims");
            await roleManager.AddPermissionClaim(superAdmin, "FixedAsset", "CWIPClaims");
            await roleManager.AddPermissionClaim(superAdmin, "FixedAsset", "DisposalClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Budget", "BudgetReappropriationClaims");
            await roleManager.AddPermissionClaim(superAdmin, "FixedAsset", "DepreciationAdjustmentClaims");
            await roleManager.AddPermissionClaimReport(superAdmin, "Finance", "ChartOfAccountClaims");
            await roleManager.AddPermissionClaimReport(superAdmin, "Report", "GeneralLedgerClaims");
            await roleManager.AddPermissionClaimReport(superAdmin, "Report", "TrialBalanceClaims");
            await roleManager.AddPermissionClaimReport(superAdmin, "Finance", "ChartOfAccountClaims");
            await roleManager.AddPermissionClaimReport(superAdmin, "Report", "BalanceSheetClaims");
            await roleManager.AddPermissionClaimReport(superAdmin, "Report", "ProfitLossClaims");
            await roleManager.AddPermissionClaimReport(superAdmin, "Budget", "BudgetReportClaims");
            await roleManager.AddPermissionClaimReport(superAdmin, "Procurement", "StockClaims");
            await roleManager.AddPermissionClaimReport(superAdmin, "FixedAsset", "FixedAssetReportClaims");
        }
        public static async Task AddPermissionClaim(this RoleManager<IdentityRole> roleManager, IdentityRole role, string module, string submodule)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = Permissions.GeneratePermissionsForModule(module, submodule);

            foreach (var permission in allPermissions)
            {
                if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
                {
                    await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                }
            }
        }
        public static async Task AddPermissionClaimReport(this RoleManager<IdentityRole> roleManager, IdentityRole role, string module, string submodule)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = Permissions.GeneratePermissionsForModuleReporting(module,submodule);
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
