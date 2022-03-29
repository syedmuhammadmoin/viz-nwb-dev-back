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

        //Profiling forms
        public static class AuthClaims
        {
            public const string View = "Permissions.AuthClaims.View";
            public const string Create = "Permissions.AuthClaims.Create";
            public const string Edit = "Permissions.AuthClaims.Edit";
            public const string Delete = "Permissions.AuthClaims.Delete";
        }
        public static class BusinessPartnerClaims
        {
            public const string View = "Permissions.BusinessPartnerClaims.View";
            public const string Create = "Permissions.BusinessPartnerClaims.Create";
            public const string Edit = "Permissions.BusinessPartnerClaims.Edit";
            public const string Delete = "Permissions.BusinessPartnerClaims.Delete";
        }

        public static class OrganizationClaims
        {
            public const string View = "Permissions.OrganizationClaims.View";
            public const string Create = "Permissions.OrganizationClaims.Create";
            public const string Edit = "Permissions.OrganizationClaims.Edit";
            public const string Delete = "Permissions.OrganizationClaims.Delete";
        }
        public static class DepartmentsClaims
        {
            public const string View = "Permissions.DepartmentsClaims.View";
            public const string Create = "Permissions.DepartmentsClaims.Create";
            public const string Edit = "Permissions.DepartmentsClaims.Edit";
            public const string Delete = "Permissions.DepartmentsClaims.Delete";
        }

        public static class LocationClaims
        {
            public const string View = "Permissions.LocationClaims.View";
            public const string Create = "Permissions.LocationClaims.Create";
            public const string Edit = "Permissions.LocationClaims.Edit";
            public const string Delete = "Permissions.LocationClaims.Delete";
        }

        public static class WarehouseClaims
        {
            public const string View = "Permissions.WarehouseClaims.View";
            public const string Create = "Permissions.WarehouseClaims.Create";
            public const string Edit = "Permissions.WarehouseClaims.Edit";
            public const string Delete = "Permissions.WarehouseClaims.Delete";
        }

        public static class BankAccountClaims
        {
            public const string View = "Permissions.BankAccountClaims.View";
            public const string Create = "Permissions.BankAccountClaims.Create";
            public const string Edit = "Permissions.BankAccountClaims.Edit";
            public const string Delete = "Permissions.BankAccountClaims.Delete";
        }

        public static class BankStatementClaims
        {
            public const string View = "Permissions.BankStatementClaims.View";
            public const string Create = "Permissions.BankStatementClaims.Create";
            public const string Edit = "Permissions.BankStatementClaims.Edit";
            public const string Delete = "Permissions.BankStatementClaims.Delete";
        }

        public static class CashAccountClaims
        {
            public const string View = "Permissions.CashAccountClaims.View";
            public const string Create = "Permissions.CashAccountClaims.Create";
            public const string Edit = "Permissions.CashAccountClaims.Edit";
            public const string Delete = "Permissions.CashAccountClaims.Delete";
        }
        //Reporting Form
        public static class ChartOfAccountClaims
        {
            public const string View = "Permissions.ChartOfAccountClaims.View";
        }

        public static class CategoriesClaims
        {
            public const string View = "Permissions.CategoriesClaims.View";
            public const string Create = "Permissions.CategoriesClaims.Create";
            public const string Edit = "Permissions.CategoriesClaims.Edit";
            public const string Delete = "Permissions.CategoriesClaims.Delete";
        }

        public static class ProductsClaims
        {
            public const string View = "Permissions.ProductsClaims.View";
            public const string Create = "Permissions.ProductsClaims.Create";
            public const string Edit = "Permissions.ProductsClaims.Edit";
            public const string Delete = "Permissions.ProductsClaims.Delete";
        }

        public static class Level4Claims
        {
            public const string View = "Permissions.Level4Claims.View";
            public const string Create = "Permissions.Level4Claims.Create";
            public const string Edit = "Permissions.Level4Claims.Edit";
            public const string Delete = "Permissions.Level4Claims.Delete";
        }
        public static class BankReconClaims
        {
            public const string View = "Permissions.BankReconClaims.View";
            public const string Create = "Permissions.BankReconClaims.Create";
            public const string Edit = "Permissions.BankReconClaims.Edit";
            public const string Delete = "Permissions.BankReconClaims.Delete";
        }
        public static class TransactionReconClaims
        {
            public const string View = "Permissions.TransactionReconClaims.View";
            public const string Create = "Permissions.TransactionReconClaims.Create";
            public const string Edit = "Permissions.TransactionReconClaims.Edit";
            public const string Delete = "Permissions.TransactionReconClaims.Delete";
        }
        public static class WorkflowClaims
        {
            public const string View = "Permissions.WorkflowClaims.View";
            public const string Create = "Permissions.WorkflowClaims.Create";
            public const string Edit = "Permissions.WorkflowClaims.Edit";
            public const string Delete = "Permissions.WorkflowClaims.Delete";
        }

        public static class WorkflowStatusClaims
        {
            public const string View = "Permissions.WorkflowStatusClaims.View";
            public const string Create = "Permissions.WorkflowStatusClaims.Create";
            public const string Edit = "Permissions.WorkflowStatusClaims.Edit";
            public const string Delete = "Permissions.WorkflowStatusClaims.Delete";
        }
        public static class BudgetClaims
        {
            public const string View = "Permissions.BudgetClaims.View";
            public const string Create = "Permissions.BudgetClaims.Create";
            public const string Edit = "Permissions.BudgetClaims.Edit";
            public const string Delete = "Permissions.BudgetClaims.Delete";
        }
        //Transaction Forms
        public static class InvoiceClaims
        {
            public const string View = "Permissions.InvoiceClaims.View";
            public const string Create = "Permissions.InvoiceClaims.Create";
            public const string Edit = "Permissions.InvoiceClaims.Edit";
            public const string Delete = "Permissions.InvoiceClaims.Delete";
        }

        public static class BillClaims
        {
            public const string View = "Permissions.BillClaims.View";
            public const string Create = "Permissions.BillClaims.Create";
            public const string Edit = "Permissions.BillClaims.Edit";
            public const string Delete = "Permissions.BillClaims.Delete";
        }

        public static class PaymentClaims
        {
            public const string View = "Permissions.PaymentClaims.View";
            public const string Create = "Permissions.PaymentClaims.Create";
            public const string Edit = "Permissions.PaymentClaims.Edit";
            public const string Delete = "Permissions.PaymentClaims.Delete";
        }

        public static class CampusClaims
        {
            public const string View = "Permissions.CampusClaims.View";
            public const string Create = "Permissions.CampusClaims.Create";
            public const string Edit = "Permissions.CampusClaims.Edit";
            public const string Delete = "Permissions.CampusClaims.Delete";
        }

        public static class CreditNoteClaims
        {
            public const string View = "Permissions.CreditNoteClaims.View";
            public const string Create = "Permissions.CreditNoteClaims.Create";
            public const string Edit = "Permissions.CreditNoteClaims.Edit";
            public const string Delete = "Permissions.CreditNoteClaims.Delete";
        }

        public static class DebitNoteClaims
        {
            public const string View = "Permissions.DebitNoteClaims.View";
            public const string Create = "Permissions.DebitNoteClaims.Create";
            public const string Edit = "Permissions.DebitNoteClaims.Edit";
            public const string Delete = "Permissions.DebitNoteClaims.Delete";
        }

        public static class JournalEntryClaims
        {
            public const string View = "Permissions.JournalEntryClaims.View";
            public const string Create = "Permissions.JournalEntryClaims.Create";
            public const string Edit = "Permissions.JournalEntryClaims.Edit";
            public const string Delete = "Permissions.JournalEntryClaims.Delete";
        }

    }
}
