using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IOrganizationRepository Organization { get; }
        IWarehouseRepository Warehouse { get; }
        ICategoryRepository Category { get; }
        IBusinessPartnerRepository BusinessPartner { get; }
        ILevel1Repository Level1 { get; }
        ILevel2Repository Level2 { get; }
        ILevel3Repository Level3 { get; }
        ILevel4Repository Level4 { get; }
        IProductRepository Product { get; }
        IJournalEntryRepository JournalEntry { get; }
        IInvoiceRepository Invoice { get; }
        IBillRepository Bill { get; }
        ICreditNoteRepository CreditNote { get; }
        IDebitNoteRepository DebitNote { get; }
        IPaymentRepository Payment { get; }
        ITransactionRepository Transaction { get; }
        ILedgerRepository Ledger { get; }
        ICashAccountRepository CashAccount { get; }
        IBankAccountRepository BankAccount { get; }
        IBankStmtRepository Bankstatement { get; }
        IBankStmtLinesRepository BankStmtLines { get; }
        IBankReconRepository BankReconciliation { get; }
        IWorkFlowRepository WorkFlow { get; }
        IWorkFlowStatusRepository WorkFlowStatus { get; }
        ITransactionReconcileRepository TransactionReconcile { get; }
        ICampusRepository Campus { get; }
        IWorkFlowTransitionRepository WorkFlowTransition { get; }
        IBudgetRepository Budget { get; }
        IPurchaseOrderRepository PurchaseOrder { get; }
        IRequisitionRepository Requisition { get; }
        IGRNRepository GRN { get; }
        IEstimatedBudgetRepository EstimatedBudget { get; }
        IDepartmentRepository Department { get; }
        IDesignationRepository Designation { get; }
        IEmployeeRepository Employee { get; }
        IPayrollItemRepository PayrollItem { get; }
        IPayrollItemEmpRepository PayrollItemEmployee { get; }
        IPayrollTransactionRepository PayrollTransaction { get; }
        IUnitOfMeasurementRepository UnitOfMeasurement { get; }
        ITaxRepository Taxes { get; }
        IIssuanceRepository Issuance { get; }
        IFileUploadRepository Fileupload { get; }
        IStockRepository Stock { get; }
        IPOToGRNLineReconcileRepository POToGRNLineReconcile { get; }
        IRequisitionToIssuanceLineReconcileRepository RequisitionToIssuanceLineReconcile { get; }
        IGRNToGoodsReturnNoteLineReconcileRepository GRNToGoodsReturnNoteLineReconcile { get; }
        IGoodsReturnNoteRepository GoodsReturnNote { get; }
        IIssuanceToIssuanceReturnLineReconcileRepository IssuanceToIssuanceReturnLineReconcile { get; }
        IIssuanceReturnRepository IssuanceReturn { get; }
        IRemarkRepository Remarks { get; }
        IRequestRepository Request { get; }
        IBidEvaluationRepository BidEvaluation { get; }
        IQuotationRepository Quotation { get; }
        ICallForQuotationRepository CallForQuotation { get; }
        IQuotationComparativeRepository QuotationComparative { get; }
        IDepreciationModelRepository DepreciationModel { get; }
        IFixedAssetRepository FixedAsset { get; }
        ICWIPRepository CWIP { get; }
        IDisposalRepository Disposal { get; }

        IBudgetReappropriationRepository BudgetReappropriation { get; } 
        IDepreciationAdjustmentRepository DepreciationAdjustment { get; }
        Task SaveAsync();
        void CreateTransaction();
        void Commit();
        void Rollback();
        void Dispose();
    }
}
