namespace Domain.Constants
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionsForModule(string module, string submodule)
        {
            return new List<string>()
            {
                $"{module}.{submodule}.Create",
                $"{module}.{submodule}.View",
                $"{module}.{submodule}.Edit",
                $"{module}.{submodule}.Delete",
            };
        }

        public static List<string> GeneratePermissionsForModuleReporting(string module, string submodule)
        {
            return new List<string>()
            {
                $"{module}.{submodule}.View",
            };
        }

        //Profiling forms
        public static class AuthClaims
        {
            public const string View = "AccessManagement.Auth.View";
            public const string Create = "AccessManagement.Auth.Create";
            public const string Edit = "AccessManagement.Auth.Edit";
            public const string Delete = "AccessManagement.Auth.Delete";
        }
        public static class BusinessPartnerClaims
        {
            public const string View = "Profiling.BusinessPartner.View";
            public const string Create = "Profiling.BusinessPartner.Create";
            public const string Edit = "Profiling.BusinessPartner.Edit";
            public const string Delete = "Profiling.BusinessPartner.Delete";
        }

        public static class OrganizationClaims
        {
            public const string View = "Profiling.Organization.View";
            public const string Create = "Profiling.Organization.Create";
            public const string Edit = "Profiling.Organization.Edit";
            public const string Delete = "Profiling.Organization.Delete";
        }
        public static class DepartmentClaims
        {
            public const string View = "Payroll.Department.View";
            public const string Create = "Payroll.Department.Create";
            public const string Edit = "Payroll.Department.Edit";
            public const string Delete = "Payroll.Department.Delete";
        }
        public static class DesignationClaims
        {
            public const string View = "Payroll.Designation.View";
            public const string Create = "Payroll.Designation.Create";
            public const string Edit = "Payroll.Designation.Edit";
            public const string Delete = "Payroll.Designation.Delete";
        }
        public static class EmployeeClaims
        {
            public const string View = "Payroll.Employee.View";
            public const string Create = "Payroll.Employee.Create";
            public const string Edit = "Payroll.Employee.Edit";
            public const string Delete = "Payroll.Employee.Delete";
        }

        public static class LocationClaims
        {
            public const string View = "Profiling.Location.View";
            public const string Create = "Profiling.Location.Create";
            public const string Edit = "Profiling.Location.Edit";
            public const string Delete = "Profiling.Location.Delete";
        }

        public static class WarehouseClaims
        {
            public const string View = "Profiling.Warehouse.View";
            public const string Create = "Profiling.Warehouse.Create";
            public const string Edit = "Profiling.Warehouse.Edit";
            public const string Delete = "Profiling.Warehouse.Delete";
        }

        public static class BankAccountClaims
        {
            public const string View = "Finance.BankAccount.View";
            public const string Create = "Finance.BankAccount.Create";
            public const string Edit = "Finance.BankAccount.Edit";
            public const string Delete = "Finance.BankAccount.Delete";
        }

        public static class BankStatementClaims
        {
            public const string View = "Finance.BankStatement.View";
            public const string Create = "Finance.BankStatement.Create";
            public const string Edit = "Finance.BankStatement.Edit";
            public const string Delete = "Finance.BankStatement.Delete";
        }

        public static class CashAccountClaims
        {
            public const string View = "Finance.CashAccount.View";
            public const string Create = "Finance.CashAccount.Create";
            public const string Edit = "Finance.CashAccount.Edit";
            public const string Delete = "Finance.CashAccount.Delete";
        }
        public static class PurchaseOrderClaims
        {
            public const string View = "Procurement.PurchaseOrder.View";
            public const string Create = "Procurement.PurchaseOrder.Create";
            public const string Edit = "Procurement.PurchaseOrder.Edit";
            public const string Delete = "Procurement.PurchaseOrder.Delete";
        }

        public static class GRNClaims
        {
            public const string View = "Procurement.GRN.View";
            public const string Create = "Procurement.GRN.Create";
            public const string Edit = "Procurement.GRN.Edit";
            public const string Delete = "Procurement.GRN.Delete";
        }
        public static class RequisitionClaims
        {
            public const string View = "Procurement.Requisition.View";
            public const string Create = "Procurement.Requisition.Create";
            public const string Edit = "Procurement.Requisition.Edit";
            public const string Delete = "Procurement.Requisition.Delete";
        }

        public static class CategoriesClaims
        {
            public const string View = "Profiling.Categories.View";
            public const string Create = "Profiling.Categories.Create";
            public const string Edit = "Profiling.Categories.Edit";
            public const string Delete = "Profiling.Categories.Delete";
        }

        public static class ProductsClaims
        {
            public const string View = "Profiling.Products.View";
            public const string Create = "Profiling.Products.Create";
            public const string Edit = "Profiling.Products.Edit";
            public const string Delete = "Profiling.Products.Delete";
        }

        public static class Level4Claims
        {
            public const string View = "Finance.Level4.View";
            public const string Create = "Finance.Level4.Create";
            public const string Edit = "Finance.Level4.Edit";
            public const string Delete = "Finance.Level4.Delete";
        }
        public static class BankReconClaims
        {
            public const string View = "Finance.BankRecon.View";
            public const string Create = "Finance.BankRecon.Create";
            public const string Edit = "Finance.BankRecon.Edit";
            public const string Delete = "Finance.BankRecon.Delete";
        }
        public static class TransactionReconClaims
        {
            public const string View = "Finance.TransactionRecon.View";
            public const string Create = "Finance.TransactionRecon.Create";
            public const string Edit = "Finance.TransactionRecon.Edit";
            public const string Delete = "Finance.TransactionRecon.Delete";
        }
        public static class WorkflowClaims
        {
            public const string View = "Workflow.Workflow.View";
            public const string Create = "Workflow.Workflow.Create";
            public const string Edit = "Workflow.Workflow.Edit";
            public const string Delete = "Workflow.Workflow.Delete";
        }

        public static class WorkflowStatusClaims
        {
            public const string View = "Workflow.WorkflowStatus.View";
            public const string Create = "Workflow.WorkflowStatus.Create";
            public const string Edit = "Workflow.WorkflowStatus.Edit";
            public const string Delete = "Workflow.WorkflowStatus.Delete";
        }
        public static class BudgetClaims
        {
            public const string View = "Budget.Budget.View";
            public const string Create = "Budget.Budget.Create";
            public const string Edit = "Budget.Budget.Edit";
            public const string Delete = "Budget.Budget.Delete";
        }
        public static class EstimatedBudgetClaims
        {
            public const string View = "Budget.EstimatedBudget.View";
            public const string Create = "Budget.EstimatedBudget.Create";
            public const string Edit = "Budget.EstimatedBudget.Edit";
            public const string Delete = "Budget.EstimatedBudget.Delete";
        }
        public static class TaxesClaims
        {
            public const string View = "Profiling.Taxes.View";
            public const string Create = "Profiling.Taxes.Create";
            public const string Edit = "Profiling.Taxes.Edit";
            public const string Delete = "Profiling.Taxes.Delete";
        }
        public static class UnitOfMeasurementClaims
        {
            public const string View = "Profiling.UnitOfMeasurement.View";
            public const string Create = "Profiling.UnitOfMeasurement.Create";
            public const string Edit = "Profiling.UnitOfMeasurement.Edit";
            public const string Delete = "Profiling.UnitOfMeasurement.Delete";
        }

        //Transaction Forms
        public static class InvoiceClaims
        {
            public const string View = "Finance.Invoice.View";
            public const string Create = "Finance.Invoice.Create";
            public const string Edit = "Finance.Invoice.Edit";
            public const string Delete = "Finance.Invoice.Delete";
        }

        public static class BillClaims
        {
            public const string View = "Finance.Bill.View";
            public const string Create = "Finance.Bill.Create";
            public const string Edit = "Finance.Bill.Edit";
            public const string Delete = "Finance.Bill.Delete";
        }

        public static class PaymentClaims
        {
            public const string View = "Finance.Payment.View";
            public const string Create = "Finance.Payment.Create";
            public const string Edit = "Finance.Payment.Edit";
            public const string Delete = "Finance.Payment.Delete";
        }

        public static class ReceiptClaims
        {
            public const string View = "Finance.Receipt.View";
            public const string Create = "Finance.Receipt.Create";
            public const string Edit = "Finance.Receipt.Edit";
            public const string Delete = "Finance.Receipt.Delete";
        }

        public static class CampusClaims
        {
            public const string View = "Profiling.Campus.View";
            public const string Create = "Profiling.Campus.Create";
            public const string Edit = "Profiling.Campus.Edit";
            public const string Delete = "Profiling.Campus.Delete";
        }

        public static class CreditNoteClaims
        {
            public const string View = "Finance.CreditNote.View";
            public const string Create = "Finance.CreditNote.Create";
            public const string Edit = "Finance.CreditNote.Edit";
            public const string Delete = "Finance.CreditNote.Delete";
        }

        public static class DebitNoteClaims
        {
            public const string View = "Finance.DebitNote.View";
            public const string Create = "Finance.DebitNote.Create";
            public const string Edit = "Finance.DebitNote.Edit";
            public const string Delete = "Finance.DebitNote.Delete";
        }

        public static class JournalEntryClaims
        {
            public const string View = "Finance.JournalEntry.View";
            public const string Create = "Finance.JournalEntry.Create";
            public const string Edit = "Finance.JournalEntry.Edit";
            public const string Delete = "Finance.JournalEntry.Delete";
        }
        public static class PayrollItemClaims
        {
            public const string View = "Payroll.PayrollItem.View";
            public const string Create = "Payroll.PayrollItem.Create";
            public const string Edit = "Payroll.PayrollItem.Edit";
            public const string Delete = "Payroll.PayrollItem.Delete";
        }
        public static class PayrollItemEmployeeClaims
        {
            public const string View = "Payroll.PayrollItemEmployee.View";
            public const string Create = "Payroll.PayrollItemEmployee.Create";
            public const string Edit = "Payroll.PayrollItemEmployee.Edit";
            public const string Delete = "Payroll.PayrollItemEmployee.Delete";
        }
        public static class PayrollTransactionClaims
        {
            public const string View = "Payroll.PayrollTransaction.View";
            public const string Create = "Payroll.PayrollTransaction.Create";
            public const string Edit = "Payroll.PayrollTransaction.Edit";
            public const string Delete = "Payroll.PayrollTransaction.Delete";
        }
        public static class PayrollPaymentClaims
        {
            public const string View = "Payroll.PayrollPayment.View";
            public const string Create = "Payroll.PayrollPayment.Create";
            public const string Edit = "Payroll.PayrollPayment.Edit";
            public const string Delete = "Payroll.PayrollPayment.Delete";
        }
        public static class IssuanceClaims
        {
            public const string View = "Procurement.Issuance.View";
            public const string Create = "Procurement.Issuance.Create";
            public const string Edit = "Procurement.Issuance.Edit";
            public const string Delete = "Procurement.Issuance.Delete";
        }
        public static class GoodsReturnNoteClaims
        {
            public const string View = "Procurement.GoodsReturnNote.View";
            public const string Create = "Procurement.GoodsReturnNote.Create";
            public const string Edit = "Procurement.GoodsReturnNote.Edit";
            public const string Delete = "Procurement.GoodsReturnNote.Delete";
        }
        public static class IssuanceReturnClaims
        {
            public const string View = "Procurement.IssuanceReturn.View";
            public const string Create = "Procurement.IssuanceReturn.Create";
            public const string Edit = "Procurement.IssuanceReturn.Edit";
            public const string Delete = "Procurement.IssuanceReturn.Delete";
        }
        public static class RequestClaims
        {
            public const string View = "Procurement.Request.View";
            public const string Create = "Procurement.Request.Create";
            public const string Edit = "Procurement.Request.Edit";
            public const string Delete = "Procurement.Request.Delete";
        }
        public static class BidEvaluationClaims
        {
            public const string View = "Procurement.BidEvaluation.View";
            public const string Create = "Procurement.BidEvaluation.Create";
            public const string Edit = "Procurement.BidEvaluation.Edit";
            public const string Delete = "Procurement.BidEvaluation.Delete";
        }
        public static class QuotationClaims
        {
            public const string View = "Procurement.Quotation.View";
            public const string Create = "Procurement.Quotation.Create";
            public const string Edit = "Procurement.Quotation.Edit";
            public const string Delete = "Procurement.Quotation.Delete";
        }
        public static class CallForQuotationClaims
        {
            public const string View = "Procurement.CallForQuotation.View";
            public const string Create = "Procurement.CallForQuotation.Create";
            public const string Edit = "Procurement.CallForQuotation.Edit";
            public const string Delete = "Procurement.CallForQuotation.Delete";
        }
        public static class QuotationComparativeClaims
        {
            public const string View = "Procurement.QuotationComparative.View";
            public const string Create = "Procurement.QuotationComparative.Create";
            public const string Edit = "Procurement.QuotationComparative.Edit";
            public const string Delete = "Procurement.QuotationComparative.Delete";
        }
        public static class DepreciationModelClaims
        {
            public const string View = "FixedAsset.DepreciationModel.View";
            public const string Create = "FixedAsset.DepreciationModel.Create";
            public const string Edit = "FixedAsset.DepreciationModel.Edit";
            public const string Delete = "FixedAsset.DepreciationModel.Delete";
        }
        public static class FixedAssetClaims
        {
            public const string View = "FixedAsset.FixedAsset.View";
            public const string Create = "FixedAsset.FixedAsset.Create";
            public const string Edit = "FixedAsset.FixedAsset.Edit";
            public const string Delete = "FixedAsset.FixedAsset.Delete";
        }
        public static class CWIPClaims
        {
            public const string View = "FixedAsset.CWIP.View";
            public const string Create = "FixedAsset.CWIP.Create";
            public const string Edit = "FixedAsset.CWIP.Edit";
            public const string Delete = "FixedAsset.CWIP.Delete";
        }
        public static class DisposalClaims
        {
            public const string View = "FixedAsset.Disposal.View";
            public const string Create = "FixedAsset.Disposal.Create";
            public const string Edit = "FixedAsset.Disposal.Edit";
            public const string Delete = "FixedAsset.Disposal.Delete";
        }
        public static class BudgetReappropriationClaims
        {
            public const string View = "Budget.BudgetReappropriation.View";
            public const string Create = "Budget.BudgetReappropriation.Create";
            public const string Edit = "Budget.BudgetReappropriation.Edit";
            public const string Delete = "Budget.BudgetReappropriation.Delete";
        }
        public static class DepreciationAdjustmentClaims
        {
            public const string View = "FixedAsset.DepreciationAdjustment.View";
            public const string Create = "FixedAsset.DepreciationAdjustment.Create";
            public const string Edit = "FixedAsset.DepreciationAdjustment.Edit";
            public const string Delete = "FixedAsset.DepreciationAdjustment.Delete";
        }

        public static class FacultyClaims
        {
            public const string View = "Admission.Faculty.View";
            public const string Create = "Admission.Faculty.Create";
            public const string Edit = "Admission.Faculty.Edit";
            public const string Delete = "Admission.Faculty.Delete";
        }

        public static class AcademicDepartmentClaims
        {
            public const string View = "Admission.AcademicDepartment.View";
            public const string Create = "Admission.AcademicDepartment.Create";
            public const string Edit = "Admission.AcademicDepartment.Edit";
            public const string Delete = "Admission.AcademicDepartment.Delete";
        }

        public static class DegreeClaims
        {
            public const string View = "Admission.Degree.View";
            public const string Create = "Admission.Degree.Create";
            public const string Edit = "Admission.Degree.Edit";
            public const string Delete = "Admission.Degree.Delete";
        }

        public static class ProgramClaims
        {
            public const string View = "Admission.Program.View";
            public const string Create = "Admission.Program.Create";
            public const string Edit = "Admission.Program.Edit";
            public const string Delete = "Admission.Program.Delete";
        }

        public static class SemesterClaims
        {
            public const string View = "Admission.Semester.View";
            public const string Create = "Admission.Semester.Create";
            public const string Edit = "Admission.Semester.Edit";
            public const string Delete = "Admission.Semester.Delete";
        }

        public static class CourseClaims
        {
            public const string View = "Admission.Course.View";
            public const string Create = "Admission.Course.Create";
            public const string Edit = "Admission.Course.Edit";
            public const string Delete = "Admission.Course.Delete";
        }

        public static class QualificationClaims
        {
            public const string View = "Admission.Qualification.View";
            public const string Create = "Admission.Qualification.Create";
            public const string Edit = "Admission.Qualification.Edit";
            public const string Delete = "Admission.Qualification.Delete";
        }

        public static class SubjectClaims
        {
            public const string View = "Admission.Subject.View";
            public const string Create = "Admission.Subject.Create";
            public const string Edit = "Admission.Subject.Edit";
            public const string Delete = "Admission.Subject.Delete";
        }

        public static class FeeItemClaims
        {
            public const string View = "Admission.FeeItem.View";
            public const string Create = "Admission.FeeItem.Create";
            public const string Edit = "Admission.FeeItem.Edit";
            public const string Delete = "Admission.FeeItem.Delete";
        }

        public static class CountryClaims
        {
            public const string View = "Admission.Country.View";
            public const string Create = "Admission.Country.Create";
            public const string Edit = "Admission.Country.Edit";
            public const string Delete = "Admission.Country.Delete";
        }

        public static class StateClaims
        {
            public const string View = "Admission.State.View";
            public const string Create = "Admission.State.Create";
            public const string Edit = "Admission.State.Edit";
            public const string Delete = "Admission.State.Delete";
        }

        public static class CityClaims
        {
            public const string View = "Admission.City.View";
            public const string Create = "Admission.City.Create";
            public const string Edit = "Admission.City.Edit";
            public const string Delete = "Admission.City.Delete";
        }

        public static class DistrictClaims
        {
            public const string View = "Admission.District.View";
            public const string Create = "Admission.District.Create";
            public const string Edit = "Admission.District.Edit";
            public const string Delete = "Admission.District.Delete";
        }

        public static class DomicileClaims
        {
            public const string View = "Admission.Domicile.View";
            public const string Create = "Admission.Domicile.Create";
            public const string Edit = "Admission.Domicile.Edit";
            public const string Delete = "Admission.Domicile.Delete";
        }

        //Reporting Form
        public static class ChartOfAccountClaims
        {
            public const string View = "Finance.ChartOfAccount.View";
        }
        public static class GeneralLedgerClaims
        {
            public const string View = "Report.GeneralLedger.View";
        }
        public static class TrialBalanceClaims
        {
            public const string View = "Report.TrialBalance.View";
        }
        public static class BalanceSheetClaims
        {
            public const string View = "Report.BalanceSheet.View";
        }
        public static class ProfitLossClaims
        {
            public const string View = "Report.ProfitLoss.View";
        }
        public static class BudgetReportClaims
        {
            public const string View = "Budget.BudgetReport.View";
        }
        public static class StockClaims
        {
            public const string View = "Procurement.Stock.View";
        }

        public static class FixedAssetReportClaims
        {
            public const string View = "FixedAsset.FixedAssetReport.View";
        }
    }
}
