using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Constants
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionsForModule(string module, string submodule)
        {
            return new List<string>()
            {
                $"Permissions.{module}.{submodule}.Create",
                $"Permissions.{module}.{submodule}.View",
                $"Permissions.{module}.{submodule}.Edit",
                $"Permissions.{module}.{submodule}.Delete",
            };
        }

        public static List<string> GeneratePermissionsForModuleReporting(string module, string submodule)
        {
            return new List<string>()
            {
                $"Permissions.{module}.{submodule}.View",
            };
        }

        //Profiling forms
        public static class AuthClaims
        {
            public const string View = "Permissions.AccessManagement.AuthClaims.View";
            public const string Create = "Permissions.AccessManagement.AuthClaims.Create";
            public const string Edit = "Permissions.AccessManagement.AuthClaims.Edit";
            public const string Delete = "Permissions.AccessManagement.AuthClaims.Delete";
        }
        public static class BusinessPartnerClaims
        {
            public const string View = "Permissions.Profiling.BusinessPartnerClaims.View";
            public const string Create = "Permissions.Profiling.BusinessPartnerClaims.Create";
            public const string Edit = "Permissions.Profiling.BusinessPartnerClaims.Edit";
            public const string Delete = "Permissions.Profiling.BusinessPartnerClaims.Delete";
        }

        public static class OrganizationClaims
        {
            public const string View = "Permissions.Profiling.OrganizationClaims.View";
            public const string Create = "Permissions.Profiling.OrganizationClaims.Create";
            public const string Edit = "Permissions.Profiling.OrganizationClaims.Edit";
            public const string Delete = "Permissions.Profiling.OrganizationClaims.Delete";
        }
        public static class DepartmentClaims
        {
            public const string View = "Permissions.Payroll.DepartmentClaims.View";
            public const string Create = "Permissions.Payroll.DepartmentClaims.Create";
            public const string Edit = "Permissions.Payroll.DepartmentClaims.Edit";
            public const string Delete = "Permissions.Payroll.DepartmentClaims.Delete";
        }
        public static class DesignationClaims
        {
            public const string View = "Permissions.Payroll.DesignationClaims.View";
            public const string Create = "Permissions.Payroll.DesignationClaims.Create";
            public const string Edit = "Permissions.Payroll.DesignationClaims.Edit";
            public const string Delete = "Permissions.Payroll.DesignationClaims.Delete";
        }
        public static class EmployeeClaims
        {
            public const string View = "Permissions.Payroll.EmployeeClaims.View";
            public const string Create = "Permissions.Payroll.EmployeeClaims.Create";
            public const string Edit = "Permissions.Payroll.EmployeeClaims.Edit";
            public const string Delete = "Permissions.Payroll.EmployeeClaims.Delete";
        }

        public static class LocationClaims
        {
            public const string View = "Permissions.Profiling.LocationClaims.View";
            public const string Create = "Permissions.Profiling.LocationClaims.Create";
            public const string Edit = "Permissions.Profiling.LocationClaims.Edit";
            public const string Delete = "Permissions.Profiling.LocationClaims.Delete";
        }

        public static class WarehouseClaims
        {
            public const string View = "Permissions.Profiling.WarehouseClaims.View";
            public const string Create = "Permissions.Profiling.WarehouseClaims.Create";
            public const string Edit = "Permissions.Profiling.WarehouseClaims.Edit";
            public const string Delete = "Permissions.Profiling.WarehouseClaims.Delete";
        }

        public static class BankAccountClaims
        {
            public const string View = "Permissions.Finance.BankAccountClaims.View";
            public const string Create = "Permissions.Finance.BankAccountClaims.Create";
            public const string Edit = "Permissions.Finance.BankAccountClaims.Edit";
            public const string Delete = "Permissions.Finance.BankAccountClaims.Delete";
        }

        public static class BankStatementClaims
        {
            public const string View = "Permissions.Finance.BankStatementClaims.View";
            public const string Create = "Permissions.Finance.BankStatementClaims.Create";
            public const string Edit = "Permissions.Finance.BankStatementClaims.Edit";
            public const string Delete = "Permissions.Finance.BankStatementClaims.Delete";
        }

        public static class CashAccountClaims
        {
            public const string View = "Permissions.Finance.CashAccountClaims.View";
            public const string Create = "Permissions.Finance.CashAccountClaims.Create";
            public const string Edit = "Permissions.Finance.CashAccountClaims.Edit";
            public const string Delete = "Permissions.Finance.CashAccountClaims.Delete";
        }
        public static class PurchaseOrderClaims
        {
            public const string View = "Permissions.Procurement.PurchaseOrderClaims.View";
            public const string Create = "Permissions.Procurement.PurchaseOrderClaims.Create";
            public const string Edit = "Permissions.Procurement.PurchaseOrderClaims.Edit";
            public const string Delete = "Permissions.Procurement.PurchaseOrderClaims.Delete";
        }

        public static class GRNClaims
        {
            public const string View = "Permissions.Procurement.GRNClaims.View";
            public const string Create = "Permissions.Procurement.GRNClaims.Create";
            public const string Edit = "Permissions.Procurement.GRNClaims.Edit";
            public const string Delete = "Permissions.Procurement.GRNClaims.Delete";
        }
        public static class RequisitionClaims
        {
            public const string View = "Permissions.Procurement.RequisitionClaims.View";
            public const string Create = "Permissions.Procurement.RequisitionClaims.Create";
            public const string Edit = "Permissions.Procurement.RequisitionClaims.Edit";
            public const string Delete = "Permissions.Procurement.RequisitionClaims.Delete";
        }

        public static class CategoriesClaims
        {
            public const string View = "Permissions.Profiling.CategoriesClaims.View";
            public const string Create = "Permissions.Profiling.CategoriesClaims.Create";
            public const string Edit = "Permissions.Profiling.CategoriesClaims.Edit";
            public const string Delete = "Permissions.Profiling.CategoriesClaims.Delete";
        }

        public static class ProductsClaims
        {
            public const string View = "Permissions.Profiling.ProductsClaims.View";
            public const string Create = "Permissions.Profiling.ProductsClaims.Create";
            public const string Edit = "Permissions.Profiling.ProductsClaims.Edit";
            public const string Delete = "Permissions.Profiling.ProductsClaims.Delete";
        }

        public static class Level4Claims
        {
            public const string View = "Permissions.Finance.Level4Claims.View";
            public const string Create = "Permissions.Finance.Level4Claims.Create";
            public const string Edit = "Permissions.Finance.Level4Claims.Edit";
            public const string Delete = "Permissions.Finance.Level4Claims.Delete";
        }
        public static class BankReconClaims
        {
            public const string View = "Permissions.Finance.BankReconClaims.View";
            public const string Create = "Permissions.Finance.BankReconClaims.Create";
            public const string Edit = "Permissions.Finance.BankReconClaims.Edit";
            public const string Delete = "Permissions.Finance.BankReconClaims.Delete";
        }
        public static class TransactionReconClaims
        {
            public const string View = "Permissions.Finance.TransactionReconClaims.View";
            public const string Create = "Permissions.Finance.TransactionReconClaims.Create";
            public const string Edit = "Permissions.Finance.TransactionReconClaims.Edit";
            public const string Delete = "Permissions.Finance.TransactionReconClaims.Delete";
        }
        public static class WorkflowClaims
        {
            public const string View = "Permissions.Workflow.WorkflowClaims.View";
            public const string Create = "Permissions.Workflow.WorkflowClaims.Create";
            public const string Edit = "Permissions.Workflow.WorkflowClaims.Edit";
            public const string Delete = "Permissions.Workflow.WorkflowClaims.Delete";
        }

        public static class WorkflowStatusClaims
        {
            public const string View = "Permissions.Workflow.WorkflowStatusClaims.View";
            public const string Create = "Permissions.Workflow.WorkflowStatusClaims.Create";
            public const string Edit = "Permissions.Workflow.WorkflowStatusClaims.Edit";
            public const string Delete = "Permissions.Workflow.WorkflowStatusClaims.Delete";
        }
        public static class BudgetClaims
        {
            public const string View = "Permissions.Budget.BudgetClaims.View";
            public const string Create = "Permissions.Budget.BudgetClaims.Create";
            public const string Edit = "Permissions.Budget.BudgetClaims.Edit";
            public const string Delete = "Permissions.Budget.BudgetClaims.Delete";
        }
        public static class EstimatedBudgetClaims
        {
            public const string View = "Permissions.Budget.EstimatedBudgetClaims.View";
            public const string Create = "Permissions.Budget.EstimatedBudgetClaims.Create";
            public const string Edit = "Permissions.Budget.EstimatedBudgetClaims.Edit";
            public const string Delete = "Permissions.Budget.EstimatedBudgetClaims.Delete";
        }
        public static class TaxesClaims
        {
            public const string View = "Permissions.Profiling.TaxesClaims.View";
            public const string Create = "Permissions.Profiling.TaxesClaims.Create";
            public const string Edit = "Permissions.Profiling.TaxesClaims.Edit";
            public const string Delete = "Permissions.Profiling.TaxesClaims.Delete";
        }
        public static class UnitOfMeasurementClaims
        {
            public const string View = "Permissions.Profiling.UnitOfMeasurementClaims.View";
            public const string Create = "Permissions.Profiling.UnitOfMeasurementClaims.Create";
            public const string Edit = "Permissions.Profiling.UnitOfMeasurementClaims.Edit";
            public const string Delete = "Permissions.Profiling.UnitOfMeasurementClaims.Delete";
        }

        //Transaction Forms
        public static class InvoiceClaims
        {
            public const string View = "Permissions.Finance.InvoiceClaims.View";
            public const string Create = "Permissions.Finance.InvoiceClaims.Create";
            public const string Edit = "Permissions.Finance.InvoiceClaims.Edit";
            public const string Delete = "Permissions.Finance.InvoiceClaims.Delete";
        }

        public static class BillClaims
        {
            public const string View = "Permissions.Finance.BillClaims.View";
            public const string Create = "Permissions.Finance.BillClaims.Create";
            public const string Edit = "Permissions.Finance.BillClaims.Edit";
            public const string Delete = "Permissions.Finance.BillClaims.Delete";
        }

        public static class PaymentClaims
        {
            public const string View = "Permissions.Finance.PaymentClaims.View";
            public const string Create = "Permissions.Finance.PaymentClaims.Create";
            public const string Edit = "Permissions.Finance.PaymentClaims.Edit";
            public const string Delete = "Permissions.Finance.PaymentClaims.Delete";
        }

        public static class ReceiptClaims
        {
            public const string View = "Permissions.Finance.ReceiptClaims.View";
            public const string Create = "Permissions.Finance.ReceiptClaims.Create";
            public const string Edit = "Permissions.Finance.ReceiptClaims.Edit";
            public const string Delete = "Permissions.Finance.ReceiptClaims.Delete";
        }

        public static class CampusClaims
        {
            public const string View = "Permissions.Profiling.CampusClaims.View";
            public const string Create = "Permissions.Profiling.CampusClaims.Create";
            public const string Edit = "Permissions.Profiling.CampusClaims.Edit";
            public const string Delete = "Permissions.Profiling.CampusClaims.Delete";
        }

        public static class CreditNoteClaims
        {
            public const string View = "Permissions.Finance.CreditNoteClaims.View";
            public const string Create = "Permissions.Finance.CreditNoteClaims.Create";
            public const string Edit = "Permissions.Finance.CreditNoteClaims.Edit";
            public const string Delete = "Permissions.Finance.CreditNoteClaims.Delete";
        }

        public static class DebitNoteClaims
        {
            public const string View = "Permissions.Finance.DebitNoteClaims.View";
            public const string Create = "Permissions.Finance.DebitNoteClaims.Create";
            public const string Edit = "Permissions.Finance.DebitNoteClaims.Edit";
            public const string Delete = "Permissions.Finance.DebitNoteClaims.Delete";
        }

        public static class JournalEntryClaims
        {
            public const string View = "Permissions.Finance.JournalEntryClaims.View";
            public const string Create = "Permissions.Finance.JournalEntryClaims.Create";
            public const string Edit = "Permissions.Finance.JournalEntryClaims.Edit";
            public const string Delete = "Permissions.Finance.JournalEntryClaims.Delete";
        }
        public static class PayrollItemClaims
        {
            public const string View = "Permissions.Payroll.PayrollItemClaims.View";
            public const string Create = "Permissions.Payroll.PayrollItemClaims.Create";
            public const string Edit = "Permissions.Payroll.PayrollItemClaims.Edit";
            public const string Delete = "Permissions.Payroll.PayrollItemClaims.Delete";
        }
        public static class PayrollItemEmployeeClaims
        {
            public const string View = "Permissions.Payroll.PayrollItemEmployeeClaims.View";
            public const string Create = "Permissions.Payroll.PayrollItemEmployeeClaims.Create";
            public const string Edit = "Permissions.Payroll.PayrollItemEmployeeClaims.Edit";
            public const string Delete = "Permissions.Payroll.PayrollItemEmployeeClaims.Delete";
        }
        public static class PayrollTransactionClaims
        {
            public const string View = "Permissions.Payroll.PayrollTransactionClaims.View";
            public const string Create = "Permissions.Payroll.PayrollTransactionClaims.Create";
            public const string Edit = "Permissions.Payroll.PayrollTransactionClaims.Edit";
            public const string Delete = "Permissions.Payroll.PayrollTransactionClaims.Delete";
        }
        public static class PayrollPaymentClaims
        {
            public const string View = "Permissions.Payroll.PayrollPaymentClaims.View";
            public const string Create = "Permissions.Payroll.PayrollPaymentClaims.Create";
            public const string Edit = "Permissions.Payroll.PayrollPaymentClaims.Edit";
            public const string Delete = "Permissions.Payroll.PayrollPaymentClaims.Delete";
        }
        public static class IssuanceClaims
        {
            public const string View = "Permissions.Procurement.IssuanceClaims.View";
            public const string Create = "Permissions.Procurement.IssuanceClaims.Create";
            public const string Edit = "Permissions.Procurement.IssuanceClaims.Edit";
            public const string Delete = "Permissions.Procurement.IssuanceClaims.Delete";
        }
        public static class GoodsReturnNoteClaims
        {
            public const string View = "Permissions.Procurement.GoodsReturnNoteClaims.View";
            public const string Create = "Permissions.Procurement.GoodsReturnNoteClaims.Create";
            public const string Edit = "Permissions.Procurement.GoodsReturnNoteClaims.Edit";
            public const string Delete = "Permissions.Procurement.GoodsReturnNoteClaims.Delete";
        }
        public static class IssuanceReturnClaims
        {
            public const string View = "Permissions.Procurement.IssuanceReturnClaims.View";
            public const string Create = "Permissions.Procurement.IssuanceReturnClaims.Create";
            public const string Edit = "Permissions.Procurement.IssuanceReturnClaims.Edit";
            public const string Delete = "Permissions.Procurement.IssuanceReturnClaims.Delete";
        }
        public static class RequestClaims
        {
            public const string View = "Permissions.Procurement.RequestClaims.View";
            public const string Create = "Permissions.Procurement.RequestClaims.Create";
            public const string Edit = "Permissions.Procurement.RequestClaims.Edit";
            public const string Delete = "Permissions.Procurement.RequestClaims.Delete";
        }
        public static class BidEvaluationClaims
        {
            public const string View = "Permissions.Procurement.BidEvaluationClaims.View";
            public const string Create = "Permissions.Procurement.BidEvaluationClaims.Create";
            public const string Edit = "Permissions.Procurement.BidEvaluationClaims.Edit";
            public const string Delete = "Permissions.Procurement.BidEvaluationClaims.Delete";
        }
        //Reporting Form
        public static class ChartOfAccountClaims
        {
            public const string View = "Permissions.Finance.ChartOfAccountClaims.View";
        }
        public static class GeneralLedgerClaims
        {
            public const string View = "Permissions.Report.GeneralLedgerClaims.View";
        }
        public static class TrialBalanceClaims
        {
            public const string View = "Permissions.Report.TrialBalanceClaims.View";
        }
        public static class BalanceSheetClaims
        {
            public const string View = "Permissions.Report.BalanceSheetClaims.View";
        }
        public static class ProfitLossClaims
        {
            public const string View = "Permissions.Report.ProfitLossClaims.View";
        }
        public static class BudgetReportClaims
        {
            public const string View = "Permissions.Budget.BudgetReportClaims.View";
        }
        public static class StockClaims
        {
            public const string View = "Permissions.Procurement.StockClaims.View";
        }
    }
}
