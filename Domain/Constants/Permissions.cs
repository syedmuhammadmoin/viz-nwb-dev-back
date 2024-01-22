namespace Domain.Constants
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionsForModule(string module, string submodule)
        {
            return new List<string>()
            {
                $"{module}.{submodule}.CR",
                $"{module}.{submodule}.VW",
                $"{module}.{submodule}.ED",
                $"{module}.{submodule}.DL",
            };
        }

        public static List<string> GeneratePermissionsForModuleReporting(string module, string submodule)
        {
            return new List<string>()
            {
                $"{module}.{submodule}.VW",
            };
        }


       


        //Profiling forms
        public static class AuthClaims
        {
            public const string View = "AccessManagement.Auth.VW";
            public const string Create = "AccessManagement.Auth.CR";
            public const string Edit = "AccessManagement.Auth.ED";
            public const string Delete = "AccessManagement.Auth.DL";
        }
        public static class BusinessPartnerClaims
        {
            public const string View = "Profiling.BusinessPartner.VW";
            public const string Create = "Profiling.BusinessPartner.CR";
            public const string Edit = "Profiling.BusinessPartner.ED";
            public const string Delete = "Profiling.BusinessPartner.DL";
        }

        public static class OrganizationClaims
        {
            public const string View = "Profiling.Organization.VW";
            public const string Create = "Profiling.Organization.CR";
            public const string Edit = "Profiling.Organization.ED";
            public const string Delete = "Profiling.Organization.DL";
        }
        public static class DepartmentClaims
        {
            public const string View = "Payroll.Department.VW";
            public const string Create = "Payroll.Department.CR";
            public const string Edit = "Payroll.Department.ED";
            public const string Delete = "Payroll.Department.DL";
        }
        public static class DesignationClaims
        {
            public const string View = "Payroll.Designation.VW";
            public const string Create = "Payroll.Designation.CR";
            public const string Edit = "Payroll.Designation.ED";
            public const string Delete = "Payroll.Designation.DL";
        }
        public static class EmployeeClaims
        {
            public const string View = "Payroll.Employee.VW";
            public const string Create = "Payroll.Employee.CR";
            public const string Edit = "Payroll.Employee.ED";
            public const string Delete = "Payroll.Employee.DL";
        }

        public static class LocationClaims
        {
            public const string View = "Profiling.Location.VW";
            public const string Create = "Profiling.Location.CR";
            public const string Edit = "Profiling.Location.ED";
            public const string Delete = "Profiling.Location.DL";
        }

        public static class WarehouseClaims
        {
            public const string View = "Profiling.Warehouse.VW";
            public const string Create = "Profiling.Warehouse.CR";
            public const string Edit = "Profiling.Warehouse.ED";
            public const string Delete = "Profiling.Warehouse.DL";
        }

        public static class BankAccountClaims
        {
            public const string View = "Finance.BankAccount.VW";
            public const string Create = "Finance.BankAccount.CR";
            public const string Edit = "Finance.BankAccount.ED";
            public const string Delete = "Finance.BankAccount.DL";
        }

        public static class BankStatementClaims
        {
            public const string View = "Finance.BankStatement.VW";
            public const string Create = "Finance.BankStatement.CR";
            public const string Edit = "Finance.BankStatement.ED";
            public const string Delete = "Finance.BankStatement.DL";
        }

        public static class CashAccountClaims
        {
            public const string View = "Finance.CashAccount.VW";
            public const string Create = "Finance.CashAccount.CR";
            public const string Edit = "Finance.CashAccount.ED";
            public const string Delete = "Finance.CashAccount.DL";
        }
        public static class PurchaseOrderClaims
        {
            public const string View = "Procurement.PurchaseOrder.VW";
            public const string Create = "Procurement.PurchaseOrder.CR";
            public const string Edit = "Procurement.PurchaseOrder.ED";
            public const string Delete = "Procurement.PurchaseOrder.DL";
        }

        public static class GRNClaims
        {
            public const string View = "Procurement.GRN.VW";
            public const string Create = "Procurement.GRN.CR";
            public const string Edit = "Procurement.GRN.ED";
            public const string Delete = "Procurement.GRN.DL";
        }
        public static class RequisitionClaims
        {
            public const string View = "Procurement.Requisition.VW";
            public const string Create = "Procurement.Requisition.CR";
            public const string Edit = "Procurement.Requisition.ED";
            public const string Delete = "Procurement.Requisition.DL";
        }

        public static class CategoriesClaims
        {
            public const string View = "Profiling.Categories.VW";
            public const string Create = "Profiling.Categories.CR";
            public const string Edit = "Profiling.Categories.ED";
            public const string Delete = "Profiling.Categories.DL";
        }

        public static class ProductsClaims
        {
            public const string View = "Profiling.Products.VW";
            public const string Create = "Profiling.Products.CR";
            public const string Edit = "Profiling.Products.ED";
            public const string Delete = "Profiling.Products.DL";
        }

        public static class Level4Claims
        {
            public const string View = "Finance.Level4.VW";
            public const string Create = "Finance.Level4.CR";
            public const string Edit = "Finance.Level4.ED";
            public const string Delete = "Finance.Level4.DL";
        }
        public static class BankReconClaims
        {
            public const string View = "Finance.BankRecon.VW";
            public const string Create = "Finance.BankRecon.CR";
            public const string Edit = "Finance.BankRecon.ED";
            public const string Delete = "Finance.BankRecon.DL";
        }
        public static class TransactionReconClaims
        {
            public const string View = "Finance.TransactionRecon.VW";
            public const string Create = "Finance.TransactionRecon.CR";
            public const string Edit = "Finance.TransactionRecon.ED";
            public const string Delete = "Finance.TransactionRecon.DL";
        }
        public static class WorkflowClaims
        {
            public const string View = "Workflow.Workflow.VW";
            public const string Create = "Workflow.Workflow.CR";
            public const string Edit = "Workflow.Workflow.ED";
            public const string Delete = "Workflow.Workflow.DL";
        }

        public static class WorkflowStatusClaims
        {
            public const string View = "Workflow.WorkflowStatus.VW";
            public const string Create = "Workflow.WorkflowStatus.CR";
            public const string Edit = "Workflow.WorkflowStatus.ED";
            public const string Delete = "Workflow.WorkflowStatus.DL";
        }
        public static class BudgetClaims
        {
            public const string View = "Budget.Budget.VW";
            public const string Create = "Budget.Budget.CR";
            public const string Edit = "Budget.Budget.ED";
            public const string Delete = "Budget.Budget.DL";
        }
        public static class EstimatedBudgetClaims
        {
            public const string View = "Budget.EstimatedBudget.VW";
            public const string Create = "Budget.EstimatedBudget.CR";
            public const string Edit = "Budget.EstimatedBudget.ED";
            public const string Delete = "Budget.EstimatedBudget.DL";
        }
        public static class TaxesClaims
        {
            public const string View = "Profiling.Taxes.VW";
            public const string Create = "Profiling.Taxes.CR";
            public const string Edit = "Profiling.Taxes.ED";
            public const string Delete = "Profiling.Taxes.DL";
        }
        public static class UnitOfMeasurementClaims
        {
            public const string View = "Profiling.UnitOfMeasurement.VW";
            public const string Create = "Profiling.UnitOfMeasurement.CR";
            public const string Edit = "Profiling.UnitOfMeasurement.ED";
            public const string Delete = "Profiling.UnitOfMeasurement.DL";
        }

        //Transaction Forms
        public static class InvoiceClaims
        {
            public const string View = "Finance.Invoice.VW";
            public const string Create = "Finance.Invoice.CR";
            public const string Edit = "Finance.Invoice.ED";
            public const string Delete = "Finance.Invoice.DL";
        }

        public static class BillClaims
        {
            public const string View = "Finance.Bill.VW";
            public const string Create = "Finance.Bill.CR";
            public const string Edit = "Finance.Bill.ED";
            public const string Delete = "Finance.Bill.DL";
        }

        public static class PaymentClaims
        {
            public const string View = "Finance.Payment.VW";
            public const string Create = "Finance.Payment.CR";
            public const string Edit = "Finance.Payment.ED";
            public const string Delete = "Finance.Payment.DL";
        }

        public static class ReceiptClaims
        {
            public const string View = "Finance.Receipt.VW";
            public const string Create = "Finance.Receipt.CR";
            public const string Edit = "Finance.Receipt.ED";
            public const string Delete = "Finance.Receipt.DL";
        }

        public static class CampusClaims
        {
            public const string View = "Profiling.Campus.VW";
            public const string Create = "Profiling.Campus.CR";
            public const string Edit = "Profiling.Campus.ED";
            public const string Delete = "Profiling.Campus.DL";
        }

        public static class CreditNoteClaims
        {
            public const string View = "Finance.CreditNote.VW";
            public const string Create = "Finance.CreditNote.CR";
            public const string Edit = "Finance.CreditNote.ED";
            public const string Delete = "Finance.CreditNote.DL";
        }

        public static class DebitNoteClaims
        {
            public const string View = "Finance.DebitNote.VW";
            public const string Create = "Finance.DebitNote.CR";
            public const string Edit = "Finance.DebitNote.ED";
            public const string Delete = "Finance.DebitNote.DL";
        }

        public static class JournalEntryClaims
        {
            public const string View = "Finance.JournalEntry.VW";
            public const string Create = "Finance.JournalEntry.CR";
            public const string Edit = "Finance.JournalEntry.ED";
            public const string Delete = "Finance.JournalEntry.DL";
        }
        public static class PettyCashClaims
        {
            public const string View = "Finance.PettyCash.VW";
            public const string Create = "Finance.PettyCash.CR";
            public const string Edit = "Finance.PettyCash.ED";
            public const string Delete = "Finance.PettyCash.DL";
        }
        public static class PayrollItemClaims
        {
            public const string View = "Payroll.PayrollItem.VW";
            public const string Create = "Payroll.PayrollItem.CR";
            public const string Edit = "Payroll.PayrollItem.ED";
            public const string Delete = "Payroll.PayrollItem.DL";
        }
        public static class PayrollItemEmployeeClaims
        {
            public const string View = "Payroll.PayrollItemEmployee.VW";
            public const string Create = "Payroll.PayrollItemEmployee.CR";
            public const string Edit = "Payroll.PayrollItemEmployee.ED";
            public const string Delete = "Payroll.PayrollItemEmployee.DL";
        }
        public static class PayrollTransactionClaims
        {
            public const string View = "Payroll.PayrollTransaction.VW";
            public const string Create = "Payroll.PayrollTransaction.CR";
            public const string Edit = "Payroll.PayrollTransaction.ED";
            public const string Delete = "Payroll.PayrollTransaction.DL";
        }
        public static class PayrollPaymentClaims
        {
            public const string View = "Payroll.PayrollPayment.VW";
            public const string Create = "Payroll.PayrollPayment.CR";
            public const string Edit = "Payroll.PayrollPayment.ED";
            public const string Delete = "Payroll.PayrollPayment.DL";
        }
        public static class IssuanceClaims
        {
            public const string View = "Procurement.Issuance.VW";
            public const string Create = "Procurement.Issuance.CR";
            public const string Edit = "Procurement.Issuance.ED";
            public const string Delete = "Procurement.Issuance.DL";
        }
        public static class GoodsReturnNoteClaims
        {
            public const string View = "Procurement.GoodsReturnNote.VW";
            public const string Create = "Procurement.GoodsReturnNote.CR";
            public const string Edit = "Procurement.GoodsReturnNote.ED";
            public const string Delete = "Procurement.GoodsReturnNote.DL";
        }
        public static class IssuanceReturnClaims
        {
            public const string View = "Procurement.IssuanceReturn.VW";
            public const string Create = "Procurement.IssuanceReturn.CR";
            public const string Edit = "Procurement.IssuanceReturn.ED";
            public const string Delete = "Procurement.IssuanceReturn.DL";
        }
        public static class RequestClaims
        {
            public const string View = "Procurement.Request.VW";
            public const string Create = "Procurement.Request.CR";
            public const string Edit = "Procurement.Request.ED";
            public const string Delete = "Procurement.Request.DL";
        }
        public static class BidEvaluationClaims
        {
            public const string View = "Procurement.BidEvaluation.VW";
            public const string Create = "Procurement.BidEvaluation.CR";
            public const string Edit = "Procurement.BidEvaluation.ED";
            public const string Delete = "Procurement.BidEvaluation.DL";
        }
        public static class QuotationClaims
        {
            public const string View = "Procurement.Quotation.VW";
            public const string Create = "Procurement.Quotation.CR";
            public const string Edit = "Procurement.Quotation.ED";
            public const string Delete = "Procurement.Quotation.DL";
        }
        public static class CallForQuotationClaims
        {
            public const string View = "Procurement.CallForQuotation.VW";
            public const string Create = "Procurement.CallForQuotation.CR";
            public const string Edit = "Procurement.CallForQuotation.ED";
            public const string Delete = "Procurement.CallForQuotation.DL";
        }
        public static class QuotationComparativeClaims
        {
            public const string View = "Procurement.QuotationComparative.VW";
            public const string Create = "Procurement.QuotationComparative.CR";
            public const string Edit = "Procurement.QuotationComparative.ED";
            public const string Delete = "Procurement.QuotationComparative.DL";
        }
        public static class DepreciationModelClaims
        {
            public const string View = "FixedAsset.DepreciationModel.VW";
            public const string Create = "FixedAsset.DepreciationModel.CR";
            public const string Edit = "FixedAsset.DepreciationModel.ED";
            public const string Delete = "FixedAsset.DepreciationModel.DL";
        }
        public static class FixedAssetClaims
        {
            public const string View = "FixedAsset.FixedAsset.VW";
            public const string Create = "FixedAsset.FixedAsset.CR";
            public const string Edit = "FixedAsset.FixedAsset.ED";
            public const string Delete = "FixedAsset.FixedAsset.DL";
        }
        public static class CWIPClaims
        {
            public const string View = "FixedAsset.CWIP.VW";
            public const string Create = "FixedAsset.CWIP.CR";
            public const string Edit = "FixedAsset.CWIP.ED";
            public const string Delete = "FixedAsset.CWIP.DL";
        }
        public static class DisposalClaims
        {
            public const string View = "FixedAsset.Disposal.VW";
            public const string Create = "FixedAsset.Disposal.CR";
            public const string Edit = "FixedAsset.Disposal.ED";
            public const string Delete = "FixedAsset.Disposal.DL";
        }
        public static class BudgetReappropriationClaims
        {
            public const string View = "Budget.BudgetReappropriation.VW";
            public const string Create = "Budget.BudgetReappropriation.CR";
            public const string Edit = "Budget.BudgetReappropriation.ED";
            public const string Delete = "Budget.BudgetReappropriation.DL";
        }
        public static class DepreciationAdjustmentClaims
        {
            public const string View = "FixedAsset.DepreciationAdjustment.VW";
            public const string Create = "FixedAsset.DepreciationAdjustment.CR";
            public const string Edit = "FixedAsset.DepreciationAdjustment.ED";
            public const string Delete = "FixedAsset.DepreciationAdjustment.DL";
        }

        public static class FacultyClaims
        {
            public const string View = "Admission.Faculty.VW";
            public const string Create = "Admission.Faculty.CR";
            public const string Edit = "Admission.Faculty.ED";
            public const string Delete = "Admission.Faculty.DL";
        }

        public static class AcademicDepartmentClaims
        {
            public const string View = "Admission.AcademicDepartment.VW";
            public const string Create = "Admission.AcademicDepartment.CR";
            public const string Edit = "Admission.AcademicDepartment.ED";
            public const string Delete = "Admission.AcademicDepartment.DL";
        }

        public static class DegreeClaims
        {
            public const string View = "Admission.Degree.VW";
            public const string Create = "Admission.Degree.CR";
            public const string Edit = "Admission.Degree.ED";
            public const string Delete = "Admission.Degree.DL";
        }

        public static class ProgramClaims
        {
            public const string View = "Admission.Program.VW";
            public const string Create = "Admission.Program.CR";
            public const string Edit = "Admission.Program.ED";
            public const string Delete = "Admission.Program.DL";
        }

        public static class SemesterClaims
        {
            public const string View = "Admission.Semester.VW";
            public const string Create = "Admission.Semester.CR";
            public const string Edit = "Admission.Semester.ED";
            public const string Delete = "Admission.Semester.DL";
        }

        public static class CourseClaims
        {
            public const string View = "Admission.Course.VW";
            public const string Create = "Admission.Course.CR";
            public const string Edit = "Admission.Course.ED";
            public const string Delete = "Admission.Course.DL";
        }

        public static class QualificationClaims
        {
            public const string View = "Admission.Qualification.VW";
            public const string Create = "Admission.Qualification.CR";
            public const string Edit = "Admission.Qualification.ED";
            public const string Delete = "Admission.Qualification.DL";
        }

        public static class SubjectClaims
        {
            public const string View = "Admission.Subject.VW";
            public const string Create = "Admission.Subject.CR";
            public const string Edit = "Admission.Subject.ED";
            public const string Delete = "Admission.Subject.DL";
        }

        public static class FeeItemClaims
        {
            public const string View = "Admission.FeeItem.VW";
            public const string Create = "Admission.FeeItem.CR";
            public const string Edit = "Admission.FeeItem.ED";
            public const string Delete = "Admission.FeeItem.DL";
        }

        public static class CountryClaims
        {
            public const string View = "Admission.Country.VW";
            public const string Create = "Admission.Country.CR";
            public const string Edit = "Admission.Country.ED";
            public const string Delete = "Admission.Country.DL";
        }

        public static class StateClaims
        {
            public const string View = "Admission.State.VW";
            public const string Create = "Admission.State.CR";
            public const string Edit = "Admission.State.ED";
            public const string Delete = "Admission.State.DL";
        }

        public static class CityClaims
        {
            public const string View = "Admission.City.VW";
            public const string Create = "Admission.City.CR";
            public const string Edit = "Admission.City.ED";
            public const string Delete = "Admission.City.DL";
        }

        public static class DistrictClaims
        {
            public const string View = "Admission.District.VW";
            public const string Create = "Admission.District.CR";
            public const string Edit = "Admission.District.ED";
            public const string Delete = "Admission.District.DL";
        }

        public static class DomicileClaims
        {
            public const string View = "Admission.Domicile.VW";
            public const string Create = "Admission.Domicile.CR";
            public const string Edit = "Admission.Domicile.ED";
            public const string Delete = "Admission.Domicile.DL";
        }

        public static class ShiftClaims
        {
            public const string View = "Admission.Shift.VW";
            public const string Create = "Admission.Shift.CR";
            public const string Edit = "Admission.Shift.ED";
            public const string Delete = "Admission.Shift.DL";
        }

        public static class BatchClaims
        {
            public const string View = "Admission.Batch.VW";
            public const string Create = "Admission.Batch.CR";
            public const string Edit = "Admission.Batch.ED";
            public const string Delete = "Admission.Batch.DL";
        }

        public static class AdmissionCriteriaClaims
        {
            public const string View = "Admission.AdmissionCriteria.VW";
            public const string Create = "Admission.AdmissionCriteria.CR";
            public const string Edit = "Admission.AdmissionCriteria.ED";
            public const string Delete = "Admission.AdmissionCriteria.DL";
        }

        public static class ApplicantClaims
        {
            public const string View = "Admission.Applicant.VW";
            public const string Create = "Admission.Applicant.CR";
            public const string Edit = "Admission.Applicant.ED";
            public const string Delete = "Admission.Applicant.DL";
        }

        public static class AdmissionApplicationClaims
        {
            public const string View = "Admission.AdmissionApplication.VW";
            public const string Create = "Admission.AdmissionApplication.CR";
            public const string Edit = "Admission.AdmissionApplication.ED";
            public const string Delete = "Admission.AdmissionApplication.DL";
        }
        public static class ProgramChallanTemplateClaims
        {
            public const string View = "Admission.ProgramChallanTemplate.VW";
            public const string Create = "Admission.ProgramChallanTemplate.CR";
            public const string Edit = "Admission.ProgramChallanTemplate.ED";
            public const string Delete = "Admission.ProgramChallanTemplate.DL";
        }

        //Reporting Form
        public static class ChartOfAccountClaims
        {
            public const string View = "Finance.ChartOfAccount.VW";
        }
        public static class GeneralLedgerClaims
        {
            public const string View = "Report.GeneralLedger.VW";
        }
        public static class TrialBalanceClaims
        {
            public const string View = "Report.TrialBalance.VW";
        }
        public static class BalanceSheetClaims
        {
            public const string View = "Report.BalanceSheet.VW";
        }
        public static class ProfitLossClaims
        {
            public const string View = "Report.ProfitLoss.VW";
        }
        public static class BudgetReportClaims
        {
            public const string View = "Budget.BudgetReport.VW";
        }
        public static class StockClaims
        {
            public const string View = "Procurement.Stock.VW";
        }

        public static class FixedAssetReportClaims
        {
            public const string View = "FixedAsset.FixedAssetReport.VW";
        }
        public  struct DashboardProfitLossClaims
        {
            public const string View = "Dashboard.ProfitLossSummary.VW";
        }
        public struct DashboardBalanceSheetClaims
        {
            public const string View = "Dashboard.BalanceSheetSummary.VW";
        }
        public struct DashboardBankBalanceClaims
        {
            public const string View = "Dashboard.BankBalance.VW";
        }
    }
}
