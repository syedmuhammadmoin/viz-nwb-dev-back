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
        public static List<string> GeneratePermissionsForModule(string module)
        {
            return new List<string>()
            {
                $"Permissions.{module}.CR",
                $"Permissions.{module}.VW",
                $"Permissions.{module}.ED",
                $"Permissions.{module}.DL",
            };
        }
        public static List<string> GeneratePermissionsForModuleReporting(string module)
        {
            return new List<string>()
            {
                $"Permissions.{module}.VW",
            };
        }
        public static List<string> GeneratePermissionsForModuleReporting(string module, string submodule)
        {
            return new List<string>()
            {
                $"{module}.{submodule}.VW",
            };
        }



        public static class AuthClaims
        {
            public const string View = "Permissions.AuthClaims.VW";
            public const string Create = "Permissions.AuthClaims.CR";
            public const string Edit = "Permissions.AuthClaims.ED";
            public const string Delete = "Permissions.AuthClaims.DL";
        }
        public static class BusinessPartnerClaims
        {
            public const string View = "Permissions.BusinessPartnerClaims.VW";
            public const string Create = "Permissions.BusinessPartnerClaims.CR";
            public const string Edit = "Permissions.BusinessPartnerClaims.ED";
            public const string Delete = "Permissions.BusinessPartnerClaims.DL";
        }
        public static class CustomerClaims
        {
            public const string View = "Permissions.CustomerClaims.VW";
            public const string Create = "Permissions.CustomerClaims.CR";
            public const string Edit = "Permissions.CustomerClaims.ED";
            public const string Delete = "Permissions.CustomerClaims.DL";
        }
        public static class VendorClaims
        {
            public const string View = "Permissions.VendorClaims.VW";
            public const string Create = "Permissions.VendorClaims.CR";
            public const string Edit = "Permissions.VendorClaims.ED";
            public const string Delete = "Permissions.VendorClaims.DL";
        }

        public static class OrganizationClaims
        {
            public const string View = "Permissions.OrganizationClaims.VW";
            public const string Create = "Permissions.OrganizationClaims.CR";
            public const string Edit = "Permissions.OrganizationClaims.ED";
            public const string Delete = "Permissions.OrganizationClaims.DL";
        }
        public static class DepartmentsClaims
        {
            public const string View = "Permissions.DepartmentsClaims.VW";
            public const string Create = "Permissions.DepartmentsClaims.CR";
            public const string Edit = "Permissions.DepartmentsClaims.ED";
            public const string Delete = "Permissions.DepartmentsClaims.DL";
        }

        public static class LocationClaims
        {
            public const string View = "Permissions.LocationClaims.VW";
            public const string Create = "Permissions.LocationClaims.CR";
            public const string Edit = "Permissions.LocationClaims.ED";
            public const string Delete = "Permissions.LocationClaims.DL";
        }

        public static class WarehouseClaims
        {
            public const string View = "Permissions.WarehouseClaims.VW";
            public const string Create = "Permissions.WarehouseClaims.CR";
            public const string Edit = "Permissions.WarehouseClaims.ED";
            public const string Delete = "Permissions.WarehouseClaims.DL";
        }

        public static class BankAccountClaims
        {
            public const string View = "Permissions.BankAccountClaims.VW";
            public const string Create = "Permissions.BankAccountClaims.CR";
            public const string Edit = "Permissions.BankAccountClaims.ED";
            public const string Delete = "Permissions.BankAccountClaims.DL";
        }

        public static class BankStatementClaims
        {
            public const string View = "Permissions.BankStatementClaims.VW";
            public const string Create = "Permissions.BankStatementClaims.CR";
            public const string Edit = "Permissions.BankStatementClaims.ED";
            public const string Delete = "Permissions.BankStatementClaims.DL";
        }

        public static class CashAccountClaims
        {
            public const string View = "Permissions.CashAccountClaims.VW";
            public const string Create = "Permissions.CashAccountClaims.CR";
            public const string Edit = "Permissions.CashAccountClaims.ED";
            public const string Delete = "Permissions.CashAccountClaims.DL";
        }

        public static class CategoriesClaims
        {
            public const string View = "Permissions.CategoriesClaims.VW";
            public const string Create = "Permissions.CategoriesClaims.CR";
            public const string Edit = "Permissions.CategoriesClaims.ED";
            public const string Delete = "Permissions.CategoriesClaims.DL";
        }

        public static class ProductsClaims
        {
            public const string View = "Permissions.ProductsClaims.VW";
            public const string Create = "Permissions.ProductsClaims.CR";
            public const string Edit = "Permissions.ProductsClaims.ED";
            public const string Delete = "Permissions.ProductsClaims.DL";
        }

        public static class Level4Claims
        {
            public const string View = "Permissions.Level4Claims.VW";
            public const string Create = "Permissions.Level4Claims.CR";
            public const string Edit = "Permissions.Level4Claims.ED";
            public const string Delete = "Permissions.Level4Claims.DL";
        }
        public static class BankReconClaims
        {
            public const string View = "Permissions.BankReconClaims.VW";
            public const string Create = "Permissions.BankReconClaims.CR";
            public const string Edit = "Permissions.BankReconClaims.ED";
            public const string Delete = "Permissions.BankReconClaims.DL";
        }
        public static class TransactionReconClaims
        {
            public const string View = "Permissions.TransactionReconClaims.VW";
            public const string Create = "Permissions.TransactionReconClaims.CR";
            public const string Edit = "Permissions.TransactionReconClaims.ED";
            public const string Delete = "Permissions.TransactionReconClaims.DL";
        }

        //Transaction Forms
        public static class InvoiceClaims
        {
            public const string View = "Permissions.InvoiceClaims.VW";
            public const string Create = "Permissions.InvoiceClaims.CR";
            public const string Edit = "Permissions.InvoiceClaims.ED";
            public const string Delete = "Permissions.InvoiceClaims.DL";
        }

        public static class BillClaims
        {
            public const string View = "Permissions.BillClaims.VW";
            public const string Create = "Permissions.BillClaims.CR";
            public const string Edit = "Permissions.BillClaims.ED";
            public const string Delete = "Permissions.BillClaims.DL";
        }

        public static class PaymentClaims
        {
            public const string View = "Permissions.PaymentClaims.VW";
            public const string Create = "Permissions.PaymentClaims.CR";
            public const string Edit = "Permissions.PaymentClaims.ED";
            public const string Delete = "Permissions.PaymentClaims.DL";
        }

        public static class ReceiptClaims
        {
            public const string View = "Permissions.ReceiptClaims.VW";
            public const string Create = "Permissions.ReceiptClaims.CR";
            public const string Edit = "Permissions.ReceiptClaims.ED";
            public const string Delete = "Permissions.ReceiptClaims.DL";
        }

        public static class CreditNoteClaims
        {
            public const string View = "Permissions.CreditNoteClaims.VW";
            public const string Create = "Permissions.CreditNoteClaims.CR";
            public const string Edit = "Permissions.CreditNoteClaims.ED";
            public const string Delete = "Permissions.CreditNoteClaims.DL";
        }

        public static class DebitNoteClaims
        {
            public const string View = "Permissions.DebitNoteClaims.VW";
            public const string Create = "Permissions.DebitNoteClaims.CR";
            public const string Edit = "Permissions.DebitNoteClaims.ED";
            public const string Delete = "Permissions.DebitNoteClaims.DL";
        }

        public static class JournalEntryClaims
        {
            public const string View = "Permissions.JournalEntryClaims.VW";
            public const string Create = "Permissions.JournalEntryClaims.CR";
            public const string Edit = "Permissions.JournalEntryClaims.ED";
            public const string Delete = "Permissions.JournalEntryClaims.DL";
        }

        public static class RequisitionClaims
        {
            public const string View = "Permissions.RequisitionClaims.VW";
            public const string Create = "Permissions.RequisitionClaims.CR";
            public const string Edit = "Permissions.RequisitionClaims.ED";
            public const string Delete = "Permissions.RequisitionClaims.DL";
        }

        public static class PurchaseOrderClaims
        {
            public const string View = "Permissions.PurchaseOrderClaims.VW";
            public const string Create = "Permissions.PurchaseOrderClaims.CR";
            public const string Edit = "Permissions.PurchaseOrderClaims.ED";
            public const string Delete = "Permissions.PurchaseOrderClaims.DL";
        }

        public static class GoodsReceivingNoteClaims
        {
            public const string View = "Permissions.GoodsReceivingNoteClaims.VW";
            public const string Create = "Permissions.GoodsReceivingNoteClaims.CR";
            public const string Edit = "Permissions.GoodsReceivingNoteClaims.ED";
            public const string Delete = "Permissions.GoodsReceivingNoteClaims.DL";
        }

        public static class SalesOrderClaims
        {
            public const string View = "Permissions.SalesOrderClaims.VW";
            public const string Create = "Permissions.SalesOrderClaims.CR";
            public const string Edit = "Permissions.SalesOrderClaims.ED";
            public const string Delete = "Permissions.SalesOrderClaims.DL";
        }

        public static class GoodsDispatchNoteClaims
        {
            public const string View = "Permissions.GoodsDispatchNoteClaims.VW";
            public const string Create = "Permissions.GoodsDispatchNoteClaims.CR";
            public const string Edit = "Permissions.GoodsDispatchNoteClaims.ED";
            public const string Delete = "Permissions.GoodsDispatchNoteClaims.DL";
        }

        //Reporting Form
        public static class ChartOfAccountClaims
        {
            public const string View = "Permissions.ChartOfAccountClaims.VW";
        }
        public static class TrialBalanceClaims
        {
            public const string View = "Permissions.TrialBalanceClaims.VW";
        }
        public static class GeneralLedgerClaims
        {
            public const string View = "Permissions.GeneralLedgerClaims.VW";
        }
        public static class ProfitLossClaims
        {
            public const string View = "Permissions.ProfitLossClaims.VW";
        }
        public static class BalanceSheetClaims
        {
            public const string View = "Permissions.BalanceSheetClaims.VW";
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

      

        public static class GRNClaims
        {
            public const string View = "Procurement.GRN.VW";
            public const string Create = "Procurement.GRN.CR";
            public const string Edit = "Procurement.GRN.ED";
            public const string Delete = "Procurement.GRN.DL";
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

       

        public static class CampusClaims
        {
            public const string View = "Profiling.Campus.VW";
            public const string Create = "Profiling.Campus.CR";
            public const string Edit = "Profiling.Campus.ED";
            public const string Delete = "Profiling.Campus.DL";
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
        public static class JournalClaims
        {
            public const string View = "Permissions.JournalClaims.VW";
            public const string Create = "Permissions.JournalClaims.CR";
            public const string Edit = "Permissions.JournalClaims.ED";
            public const string Delete = "Permissions.JournalClaims.DL";
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
