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
        public static class DepartmentClaims
        {
            public const string View = "Permissions.DepartmentClaims.View";
            public const string Create = "Permissions.DepartmentClaims.Create";
            public const string Edit = "Permissions.DepartmentClaims.Edit";
            public const string Delete = "Permissions.DepartmentClaims.Delete";
        }
        public static class DesignationClaims
        {
            public const string View = "Permissions.DesignationClaims.View";
            public const string Create = "Permissions.DesignationClaims.Create";
            public const string Edit = "Permissions.DesignationClaims.Edit";
            public const string Delete = "Permissions.DesignationClaims.Delete";
        }
        public static class EmployeeClaims
        {
            public const string View = "Permissions.EmployeeClaims.View";
            public const string Create = "Permissions.EmployeeClaims.Create";
            public const string Edit = "Permissions.EmployeeClaims.Edit";
            public const string Delete = "Permissions.EmployeeClaims.Delete";
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
        public static class PurchaseOrderClaims
        {
            public const string View = "Permissions.PurchaseOrderClaims.View";
            public const string Create = "Permissions.PurchaseOrderClaims.Create";
            public const string Edit = "Permissions.PurchaseOrderClaims.Edit";
            public const string Delete = "Permissions.PurchaseOrderClaims.Delete";
        }

        public static class GRNClaims
        {
            public const string View = "Permissions.GRNClaims.View";
            public const string Create = "Permissions.GRNClaims.Create";
            public const string Edit = "Permissions.GRNClaims.Edit";
            public const string Delete = "Permissions.GRNClaims.Delete";
        }
        public static class RequisitionClaims
        {
            public const string View = "Permissions.RequisitionClaims.View";
            public const string Create = "Permissions.RequisitionClaims.Create";
            public const string Edit = "Permissions.RequisitionClaims.Edit";
            public const string Delete = "Permissions.RequisitionClaims.Delete";
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
        public static class EstimatedBudgetClaims
        {
            public const string View = "Permissions.EstimatedBudgetClaims.View";
            public const string Create = "Permissions.EstimatedBudgetClaims.Create";
            public const string Edit = "Permissions.EstimatedBudgetClaims.Edit";
            public const string Delete = "Permissions.EstimatedBudgetClaims.Delete";
        }
        public static class TaxesClaims
        {
            public const string View = "Permissions.TaxesClaims.View";
            public const string Create = "Permissions.TaxesClaims.Create";
            public const string Edit = "Permissions.TaxesClaims.Edit";
            public const string Delete = "Permissions.TaxesClaims.Delete";
        }
        public static class UnitOfMeasurementClaims
        {
            public const string View = "Permissions.UnitOfMeasurementClaims.View";
            public const string Create = "Permissions.UnitOfMeasurementClaims.Create";
            public const string Edit = "Permissions.UnitOfMeasurementClaims.Edit";
            public const string Delete = "Permissions.UnitOfMeasurementClaims.Delete";
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

        public static class ReceiptClaims
        {
            public const string View = "Permissions.ReceiptClaims.View";
            public const string Create = "Permissions.ReceiptClaims.Create";
            public const string Edit = "Permissions.ReceiptClaims.Edit";
            public const string Delete = "Permissions.ReceiptClaims.Delete";
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
        public static class PayrollItemClaims
        {
            public const string View = "Permissions.PayrollItemClaims.View";
            public const string Create = "Permissions.PayrollItemClaims.Create";
            public const string Edit = "Permissions.PayrollItemClaims.Edit";
            public const string Delete = "Permissions.PayrollItemClaims.Delete";
        }
        public static class PayrollItemEmployeeClaims
        {
            public const string View = "Permissions.PayrollItemEmployeeClaims.View";
            public const string Create = "Permissions.PayrollItemEmployeeClaims.Create";
            public const string Edit = "Permissions.PayrollItemEmployeeClaims.Edit";
            public const string Delete = "Permissions.PayrollItemEmployeeClaims.Delete";
        }
        public static class PayrollTransactionClaims
        {
            public const string View = "Permissions.PayrollTransactionClaims.View";
            public const string Create = "Permissions.PayrollTransactionClaims.Create";
            public const string Edit = "Permissions.PayrollTransactionClaims.Edit";
            public const string Delete = "Permissions.PayrollTransactionClaims.Delete";
        }
        public static class PayrollPaymentClaims
        {
            public const string View = "Permissions.PayrollPaymentClaims.View";
            public const string Create = "Permissions.PayrollPaymentClaims.Create";
            public const string Edit = "Permissions.PayrollPaymentClaims.Edit";
            public const string Delete = "Permissions.PayrollPaymentClaims.Delete";
        }
        public static class IssuanceClaims
        {
            public const string View = "Permissions.IssuanceClaims.View";
            public const string Create = "Permissions.IssuanceClaims.Create";
            public const string Edit = "Permissions.IssuanceClaims.Edit";
            public const string Delete = "Permissions.IssuanceClaims.Delete";
        }
        public static class GoodsReturnNoteClaims
        {
            public const string View = "Permissions.GoodsReturnNoteClaims.View";
            public const string Create = "Permissions.GoodsReturnNoteClaims.Create";
            public const string Edit = "Permissions.GoodsReturnNoteClaims.Edit";
            public const string Delete = "Permissions.GoodsReturnNoteClaims.Delete";
        }
        public static class IssuanceReturnClaims
        {
            public const string View = "Permissions.IssuanceReturnClaims.View";
            public const string Create = "Permissions.IssuanceReturnClaims.Create";
            public const string Edit = "Permissions.IssuanceReturnClaims.Edit";
            public const string Delete = "Permissions.IssuanceReturnClaims.Delete";
        }
        //Reporting Form
        public static class ChartOfAccountClaims
        {
            public const string View = "Permissions.ChartOfAccountClaims.View";
        }
        public static class GeneralLedgerClaims
        {
            public const string View = "Permissions.GeneralLedgerClaims.View";
        }
        public static class TrialBalanceClaims
        {
            public const string View = "Permissions.TrialBalanceClaims.View";
        }
        public static class BalanceSheetClaims
        {
            public const string View = "Permissions.BalanceSheetClaims.View";
        }
        public static class ProfitLossClaims
        {
            public const string View = "Permissions.ProfitLossClaims.View";
        }
        public static class BudgetReportClaims
        {
            public const string View = "Permissions.BudgetReportClaims.View";
        }
        public static class StockClaims
        {
            public const string View = "Permissions.StockClaims.View";
        }
    }
}
