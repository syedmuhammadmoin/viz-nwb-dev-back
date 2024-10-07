using Domain.Constants;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Infrastructure.Seeds
{
    public static class DefaultUsers
    {
        public static async Task SeedSuperAdminAsync(UserManager<User> userManager, RoleManager<Role> roleManager, string password)
        {
            var defaultUser = new User
            {
                UserName = "Muhammad",
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
                await roleManager.SeedClaimsForApplicant();
            }
        }
        public static async Task SeedSuperAdminAsync(RoleManager<Role> roleManager, Role role)
        {
            await roleManager.SeedClaimsForSuperAdmin(role);
        }
        private async static Task SeedClaimsForSuperAdmin(this RoleManager<Role> roleManager, Role superAdmin)
        {
            //var superAdmin = await roleManager.FindByIdAsync(roleId);
            await roleManager.AddPermissionClaim(superAdmin, "AuthClaims");
            await roleManager.AddPermissionClaim(superAdmin, "BusinessPartnerClaims");
            await roleManager.AddPermissionClaim(superAdmin, "CustomerClaims");
            await roleManager.AddPermissionClaim(superAdmin, "VendorClaims");
            await roleManager.AddPermissionClaim(superAdmin, "OrganizationClaims");
            await roleManager.AddPermissionClaim(superAdmin, "DepartmentsClaims");
            await roleManager.AddPermissionClaim(superAdmin, "WarehouseClaims");
            await roleManager.AddPermissionClaim(superAdmin, "LocationClaims");
            await roleManager.AddPermissionClaim(superAdmin, "BankAccountClaims");
            await roleManager.AddPermissionClaim(superAdmin, "BankStatementClaims");
            await roleManager.AddPermissionClaim(superAdmin, "CashAccountClaims");
            await roleManager.AddPermissionClaim(superAdmin, "CategoriesClaims");
            await roleManager.AddPermissionClaim(superAdmin, "ProductsClaims");
            await roleManager.AddPermissionClaim(superAdmin, "Level4Claims");
            await roleManager.AddPermissionClaim(superAdmin, "BankReconClaims");
            await roleManager.AddPermissionClaim(superAdmin, "TransactionReconClaims");
            await roleManager.AddPermissionClaim(superAdmin, "InvoiceClaims");
            await roleManager.AddPermissionClaim(superAdmin, "BillClaims");
            await roleManager.AddPermissionClaim(superAdmin, "PaymentClaims");
            await roleManager.AddPermissionClaim(superAdmin, "ReceiptClaims");
            await roleManager.AddPermissionClaim(superAdmin, "CreditNoteClaims");
            await roleManager.AddPermissionClaim(superAdmin, "DebitNoteClaims");
            await roleManager.AddPermissionClaim(superAdmin, "JournalEntryClaims");
            await roleManager.AddPermissionClaim(superAdmin, "RequisitionClaims");
            await roleManager.AddPermissionClaim(superAdmin, "PurchaseOrderClaims");
            await roleManager.AddPermissionClaim(superAdmin, "GoodsReceivingNoteClaims");
            await roleManager.AddPermissionClaim(superAdmin, "SalesOrderClaims");
            await roleManager.AddPermissionClaim(superAdmin, "GoodsDispatchNoteClaims");
            await roleManager.AddPermissionClaim(superAdmin, "GoodsDispatchNoteClaims");

            await roleManager.AddPermissionClaimReport(superAdmin, "ChartOfAccountClaims");
            await roleManager.AddPermissionClaimReport(superAdmin, "GeneralLedgerClaims");
            await roleManager.AddPermissionClaimReport(superAdmin, "ProfitLossClaims");
            await roleManager.AddPermissionClaimReport(superAdmin, "BalanceSheetClaims");
            await roleManager.AddPermissionClaimReport(superAdmin, "TrialBalanceClaims");
            await roleManager.AddPermissionClaimReport(superAdmin, "JournalClaims");
            await roleManager.AddPermissionClaimReport(superAdmin, "TaxesClaims");

            await roleManager.AddPermissionClaimReport(superAdmin, "CurrencyClaims");
            
        }
        public static async Task AddPermissionClaimReport(this RoleManager<Role> roleManager, Role role, string module)
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
        public static async Task AddPermissionClaim(this RoleManager<Role> roleManager, Role role, string module)
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
        private async static Task SeedClaimsForSuperAdmin(this RoleManager<Role> roleManager)
        {
            var superAdmin = await roleManager.FindByNameAsync("SuperAdmin");
            await roleManager.AddPermissionClaim(superAdmin, "AccessManagement", "Auth");
            await roleManager.AddPermissionClaim(superAdmin, "Profiling", "BusinessPartner");
            await roleManager.AddPermissionClaim(superAdmin, "Profiling", "Organization");
            await roleManager.AddPermissionClaim(superAdmin, "Payroll", "Department");
            await roleManager.AddPermissionClaim(superAdmin, "Payroll", "Designation");
            await roleManager.AddPermissionClaim(superAdmin, "Profiling", "Campus");
            await roleManager.AddPermissionClaim(superAdmin, "Profiling", "Warehouse");
            await roleManager.AddPermissionClaim(superAdmin, "Profiling", "Location");
            await roleManager.AddPermissionClaim(superAdmin, "Finance", "BankAccount");
            await roleManager.AddPermissionClaim(superAdmin, "Finance", "BankStatement");
            await roleManager.AddPermissionClaim(superAdmin, "Finance", "CashAccount");
            await roleManager.AddPermissionClaim(superAdmin, "Profiling", "Campus");
            await roleManager.AddPermissionClaim(superAdmin, "Profiling", "Categories");
            await roleManager.AddPermissionClaim(superAdmin, "Profiling", "Products");
            await roleManager.AddPermissionClaim(superAdmin, "Workflow", "WorkflowStatus");
            await roleManager.AddPermissionClaim(superAdmin, "Workflow", "Workflow");
            await roleManager.AddPermissionClaim(superAdmin, "Finance", "Level4");
            await roleManager.AddPermissionClaim(superAdmin, "Finance", "BankRecon");
            await roleManager.AddPermissionClaim(superAdmin, "Finance", "TransactionRecon");
            await roleManager.AddPermissionClaim(superAdmin, "Finance", "Invoice");
            await roleManager.AddPermissionClaim(superAdmin, "Finance", "Bill");
            await roleManager.AddPermissionClaim(superAdmin, "Finance", "Payment");
            await roleManager.AddPermissionClaim(superAdmin, "Finance", "CreditNote");
            await roleManager.AddPermissionClaim(superAdmin, "Finance", "DebitNote");
            await roleManager.AddPermissionClaim(superAdmin, "Finance", "JournalEntry");
            await roleManager.AddPermissionClaim(superAdmin, "Finance", "PettyCash");
            await roleManager.AddPermissionClaim(superAdmin, "Budget", "Budget");
            await roleManager.AddPermissionClaim(superAdmin, "Finance", "Receipt");
            await roleManager.AddPermissionClaim(superAdmin, "Procurement", "Requisition");
            await roleManager.AddPermissionClaim(superAdmin, "Procurement", "PurchaseOrder");
            await roleManager.AddPermissionClaim(superAdmin, "Procurement", "GRN");
            await roleManager.AddPermissionClaim(superAdmin, "Budget", "EstimatedBudget");
            await roleManager.AddPermissionClaim(superAdmin,"Payroll", "Employee");
            await roleManager.AddPermissionClaim(superAdmin, "Payroll", "PayrollItem");
            await roleManager.AddPermissionClaim(superAdmin, "Payroll", "PayrollItemEmployee");
            await roleManager.AddPermissionClaim(superAdmin, "Payroll", "PayrollTransaction");
            await roleManager.AddPermissionClaim(superAdmin, "Payroll", "PayrollPayment");
            await roleManager.AddPermissionClaim(superAdmin, "Profiling", "Taxes");
            await roleManager.AddPermissionClaim(superAdmin, "Profiling", "UnitOfMeasurement");
            await roleManager.AddPermissionClaim(superAdmin, "Procurement", "Issuance");
            await roleManager.AddPermissionClaim(superAdmin, "Procurement", "GoodsReturnNote");
            await roleManager.AddPermissionClaim(superAdmin, "Procurement", "IssuanceReturn");
            await roleManager.AddPermissionClaim(superAdmin, "Procurement", "Request");
            await roleManager.AddPermissionClaim(superAdmin, "Procurement", "BidEvaluation");
            await roleManager.AddPermissionClaim(superAdmin, "Procurement", "Quotation");
            await roleManager.AddPermissionClaim(superAdmin, "Procurement", "CallForQuotation");
            await roleManager.AddPermissionClaim(superAdmin, "Procurement", "QuotationComparative");
            await roleManager.AddPermissionClaim(superAdmin, "FixedAsset", "FixedAsset");
            await roleManager.AddPermissionClaim(superAdmin, "FixedAsset", "DepreciationModel");
            await roleManager.AddPermissionClaim(superAdmin, "FixedAsset", "CWIP");
            await roleManager.AddPermissionClaim(superAdmin, "FixedAsset", "Disposal");
            await roleManager.AddPermissionClaim(superAdmin, "Budget", "BudgetReappropriation");
            await roleManager.AddPermissionClaim(superAdmin, "FixedAsset", "DepreciationAdjustment");
            await roleManager.AddPermissionClaim(superAdmin, "Admission", "Faculty");
            await roleManager.AddPermissionClaim(superAdmin, "Admission", "AcademicDepartment");
            await roleManager.AddPermissionClaim(superAdmin, "Admission", "Degree");
            await roleManager.AddPermissionClaim(superAdmin, "Admission", "Program");
            await roleManager.AddPermissionClaim(superAdmin, "Admission", "Semester");
            await roleManager.AddPermissionClaim(superAdmin, "Admission", "Course");
            await roleManager.AddPermissionClaim(superAdmin, "Admission", "Qualification");
            await roleManager.AddPermissionClaim(superAdmin, "Admission", "Subject");
            await roleManager.AddPermissionClaim(superAdmin, "Admission", "FeeItem");
            await roleManager.AddPermissionClaim(superAdmin, "Admission", "Country");
            await roleManager.AddPermissionClaim(superAdmin, "Admission", "State");
            await roleManager.AddPermissionClaim(superAdmin, "Admission", "City");
            await roleManager.AddPermissionClaim(superAdmin, "Admission", "District");
            await roleManager.AddPermissionClaim(superAdmin, "Admission", "Domicile");
            await roleManager.AddPermissionClaim(superAdmin, "Admission", "Shift");
            await roleManager.AddPermissionClaim(superAdmin, "Admission", "Batch");
            await roleManager.AddPermissionClaim(superAdmin, "Admission", "AdmissionCriteria");
            await roleManager.AddPermissionClaim(superAdmin, "Admission", "Applicant");
            await roleManager.AddPermissionClaim(superAdmin, "Admission", "AdmissionApplication");
            await roleManager.AddPermissionClaim(superAdmin, "Admission", "ProgramChallanTemplate");


            await roleManager.AddPermissionClaimReport(superAdmin, "Finance", "ChartOfAccount");
            await roleManager.AddPermissionClaimReport(superAdmin, "Report", "GeneralLedger");
            await roleManager.AddPermissionClaimReport(superAdmin, "Report", "TrialBalance");
            await roleManager.AddPermissionClaimReport(superAdmin, "Finance", "ChartOfAccount");
            await roleManager.AddPermissionClaimReport(superAdmin, "Report", "BalanceSheet");
            await roleManager.AddPermissionClaimReport(superAdmin, "Report", "ProfitLoss");
            await roleManager.AddPermissionClaimReport(superAdmin, "Budget", "BudgetReport");
            await roleManager.AddPermissionClaimReport(superAdmin, "Procurement", "Stock");
            await roleManager.AddPermissionClaimReport(superAdmin, "FixedAsset", "FixedAssetReport");
            await roleManager.AddPermissionClaimReport(superAdmin, "Dashboard", "ProfitLossSummary");
            await roleManager.AddPermissionClaimReport(superAdmin, "Dashboard", "BalanceSheetSummary");
            await roleManager.AddPermissionClaimReport(superAdmin, "Dashboard", "BankBalance");
       
        }
        private async static Task SeedClaimsForApplicant(this RoleManager<Role> roleManager)
        {
            var applicant = await roleManager.FindByNameAsync(Roles.Applicant.ToString());
            await roleManager.AddPermissionClaim(applicant, "Admission", "AdmissionApplication");
        }
        public static async Task AddPermissionClaim(this RoleManager<Role> roleManager, Role role, string module, string submodule)
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
        public static async Task AddPermissionClaimReport(this RoleManager<Role> roleManager, Role role, string module, string submodule)
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
