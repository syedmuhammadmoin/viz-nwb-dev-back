﻿using Domain.Base;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Uow
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction _transaction;
        public ICategoryRepository Category { get; private set; }
        public IOrganizationRepository Organization { get; private set; }
        public IWarehouseRepository Warehouse { get; private set; }
        public IBusinessPartnerRepository BusinessPartner { get; private set; }
        public IProductRepository Product { get; private set; }
        public ILevel4Repository Level4 { get; private set; }
        public ILevel1Repository Level1 { get; private set; }
        public ILevel2Repository Level2 { get; private set; }
        public ILevel3Repository Level3 { get; private set; }
        public IJournalEntryRepository JournalEntry { get; private set; }
        public IPettyCashRepository PettyCash { get; private set; }
        public IInvoiceRepository Invoice { get; private set; }
        public IBillRepository Bill{ get; private set; }
        public ICreditNoteRepository CreditNote { get; private set; }
        public IDebitNoteRepository DebitNote { get; private set; }
        public IPaymentRepository Payment { get; private set; }
        public ITransactionRepository Transaction { get; private set; }
        public ILedgerRepository Ledger { get; private set; }
        public ICashAccountRepository CashAccount { get; private set; }
        public IBankAccountRepository BankAccount { get; private set; }
        public IBankStmtRepository Bankstatement { get; private set; }
        public IBankStmtLinesRepository BankStmtLines { get; private set; }
        public IBankReconRepository BankReconciliation { get; private set; }
        public IWorkFlowRepository WorkFlow { get; private set; }
        public IWorkFlowStatusRepository WorkFlowStatus { get; private set; }
        public ITransactionReconcileRepository TransactionReconcile { get; private set; }
        public ICampusRepository Campus { get; private set; }
        public IWorkFlowTransitionRepository WorkFlowTransition { get; private set; }
        public IBudgetRepository Budget { get; private set; }
        public IPurchaseOrderRepository PurchaseOrder { get; private set; }
        public IRequisitionRepository Requisition { get; private set; }
        public IGRNRepository GRN { get; private set; }
        public IEstimatedBudgetRepository EstimatedBudget { get; private set; }
        public IDepartmentRepository Department { get; private set; }
        public IDesignationRepository Designation { get; private set; }
        public IEmployeeRepository Employee { get; private set; }
        public IPayrollItemRepository PayrollItem { get; private set; }
        public IPayrollItemEmpRepository PayrollItemEmployee { get; private set; }
        public IPayrollTransactionRepository PayrollTransaction { get; private set; }
        public ITaxRepository Taxes { get; private set; }
        public IUnitOfMeasurementRepository UnitOfMeasurement { get; private set; }
        public IIssuanceRepository Issuance { get; private set; }
        public IFileUploadRepository Fileupload { get; private set; }
        public IStockRepository Stock { get; private set; }
        public IPOToGRNLineReconcileRepository POToGRNLineReconcile { get; private set; }
        public IRequisitionToIssuanceLineReconcileRepository RequisitionToIssuanceLineReconcile { get; private set; }
        public IGRNToGoodsReturnNoteLineReconcileRepository GRNToGoodsReturnNoteLineReconcile { get; private set; }
        public IGoodsReturnNoteRepository GoodsReturnNote { get; private set; }
        public IIssuanceToIssuanceReturnLineReconcileRepository IssuanceToIssuanceReturnLineReconcile { get; private set; }
        public IIssuanceReturnRepository IssuanceReturn { get; private set; }
        public IRemarkRepository Remarks { get; private set; }
        public IRequestRepository Request { get; private set; }
        public IBidEvaluationRepository BidEvaluation { get; private set; }
        public IQuotationRepository Quotation { get; private set; }
        public ICallForQuotationRepository CallForQuotation { get; private set; }
        public IQuotationComparativeRepository QuotationComparative { get; private set; }
        public IDepreciationModelRepository DepreciationModel { get; private set; }
        public IFixedAssetRepository FixedAsset { get; private set; }
        public ICWIPRepository CWIP { get; private set; }
        public IDisposalRepository Disposal { get; private set; }
        public IBudgetReappropriationRepository BudgetReappropriation { get; private set; }
        public IDepreciationAdjustmentRepository DepreciationAdjustment { get; private set; }
        public IFixedAssetLinesRepository FixedAssetLines { get; private set; }
        public IDepreciationRegisterRepository DepreciationRegister { get; private set; }
        public IFacultyRepository Faculty { get; private set; }
        public IAcademicDepartmentRepository AcademicDepartment { get; private set; }
        public IDegreeRepository Degree { get; private set; }
        public IProgramRepository Program { get; private set; }
        public ISemesterRepository Semester { get; private set; }
        public ICourseRepository Course { get; private set; }
        public IQualificationRepository Qualification { get; private set; }
        public ISubjectRepository Subject { get; private set; }
        public IFeeItemRepository FeeItem { get; private set; }
        public ICountryRepository Country { get; private set; }
        public IStateRepository State { get; private set; }
        public ICityRepository City { get; private set; }
        public IDistrictRepository District { get; private set; }
        public IDomicileRepository Domicile { get; private set; }
        public IShiftRepository Shift { get; private set; }
        public IBatchRepository Batch { get; private set; }
        public IAdmissionCriteriaRepository AdmissionCriteria { get; private set; }
        public IApplicantRepository Applicant { get; private set; }
        public IAdmissionApplicationRepository AdmissionApplication { get; private set; }
        public IProgramChallanTemplateRepository ProgramChallanTemplate { get; private set; }
        public IUsersOrganization UsersOrganization { get; private set; }
        public IInviteUser InviteUser { get; private set; }
        public IJournalRepository Journals { get; private set; }
        public IGenericRepository<BaseEntity<int>, int> GenericRepository { get; private set; }
        private readonly Dictionary<Type, object> _repositories = new();
        public ITaxGroupRepository TaxGroup { get; private set; }
        public ITaxSettingRepository TaxSetting {  get; private set; }
        public IFiscalPeriodRepository FiscalPeriod { get; private set; }
        public IFiscalPeriodSettingRepository FiscalPeriodSetting { get; private set; }
        public ICurrencySettingRepository CurrencySetting { get; private set; }
        public IAccountingSettingRepository AccountingSetting { get; private set; }
        public IGeneralSettingRepository GeneralSetting { get; private set; }


        public UnitOfWork(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            Organization = new OrganizationRepository(context);
            Warehouse = new WarehouseRepository(context);
            Category = new CategoryRepository(context);
            BusinessPartner = new BusinessPartnerRepository(context);
            Level4 = new Level4Repository(context, httpContextAccessor);
            Level3 = new Level3Repository(context);
            Level2 = new Level2Repository(context);
            Level1 = new Level1Repository(context);
            Product = new ProductRepository(context);
            JournalEntry = new JournalEntryRepository(context);
            PettyCash = new PettyCashRepository(context);
            Invoice = new InvoiceRepository(context);
            Bill = new BillRepository(context);
            CreditNote = new CreditNoteRepository(context);
            DebitNote = new DebitNoteRepository(context);
            Payment = new PaymentRepository(context);
            Transaction = new TransactionRepository(context);
            Ledger = new LedgerRepository(context, httpContextAccessor);
            CashAccount = new CashAccountRepository(context);
            BankAccount = new BankAccountRepository(context);
            Bankstatement = new BankStmtRepository(context);
            BankStmtLines = new BankStmtLinesRepository(context);
            BankReconciliation = new BankReconRepository(context);
            WorkFlow = new WorkFlowRepository(context);
            WorkFlowStatus = new WorkFlowStatusRepository(context);
            WorkFlowTransition = new WorkFlowTransitionRepository(context);
            TransactionReconcile = new TransactionReconcileRepository(context);
            Campus = new CampusRepository(context);
            Budget = new BudgetRepository(context);
            PurchaseOrder = new PurchaseOrderRepository(context);
            Requisition = new RequisitionRepository(context);
            GRN = new GRNRepository(context);
            EstimatedBudget = new EstimatedBudgetRepository(context);
            Department = new DepartmentRepository(context);
            Designation = new DesignationRepository(context);
            Employee = new EmployeeRepository(context);
            PayrollItem = new PayrollItemRepository(context);
            PayrollItemEmployee = new PayrollItemEmpRepository(context);
            PayrollTransaction = new PayrollTransactionRepository(context);
            Taxes = new TaxRepository(context);
            UnitOfMeasurement = new UnitOfMeasurementRepository(context);
            Issuance = new IssuanceRepository(context);
            Stock = new StockRepository(context);
            Fileupload = new FileUploadRepository(context); Stock = new StockRepository(context);
            POToGRNLineReconcile = new POToGRNLineReconcileRepository(context);
            RequisitionToIssuanceLineReconcile = new RequisitionToIssuanceLineReconcileRepository(context);
            GoodsReturnNote = new GoodsReturnNoteRepository(context);
            GRNToGoodsReturnNoteLineReconcile = new GRNToGoodsReturnNoteLineReconcileRepository(context);
            IssuanceToIssuanceReturnLineReconcile = new IssuanceToIssuanceReturnLineReconcileRepository(context);
            IssuanceReturn = new IssuanceReturnRepository(context);
            Remarks = new RemarkRepository(context);
            Request = new RequestRepository(context);
            BidEvaluation = new BidEvaluationRepository(context);
            Quotation = new QuotationRepository(context);
            CallForQuotation = new CallForQuotationRepository(context);
            QuotationComparative = new QuotationComparativeRepository(context);
            DepreciationModel = new DepreciationModelRepository(context);
            FixedAsset = new FixedAssetRepository(context);  
            CWIP = new CWIPRepository(context);
            Disposal = new DisposalRepository(context);
            BudgetReappropriation = new BudgetReappropriationRepository(context);
            DepreciationAdjustment = new DepreciationAdjustmentRepository(context);
            FixedAssetLines = new FixedAssetLinesRepository(context);
            DepreciationRegister = new DepreciationRegisterRepository(context);
            Faculty = new FacultyRepository(context);
            AcademicDepartment = new AcademicDepartmentRepository(context);
            Degree = new DegreeRepository(context);
            Program = new ProgramRepository(context);
            Semester = new SemesterRepository(context);
            Course = new CourseRepository(context);
            Qualification = new QualificationRepository(context);
            Subject = new SubjectRepository(context);
            FeeItem = new FeeItemRepository(context);
            Country = new CountryRepository(context);
            State = new StateRepository(context);
            City = new CityRepository(context);
            District = new DistrictRepository(context);
            Domicile = new DomicileRepository(context);
            Shift = new ShiftRepository(context);
            Batch = new BatchRepository(context);
            AdmissionCriteria = new AdmissionCriteriaRepository(context);
            Applicant = new ApplicantRepository(context);
            AdmissionApplication = new AdmissionApplicationRepository(context);
            ProgramChallanTemplate = new ProgramChallanTemplateRepository(context);
            UsersOrganization = new UsersOrganizationRepository(context);
            InviteUser = new InviteUserRepository(context);
            Journals = new JournalRepository(context);
            TaxGroup = new TaxGroupRepository(context);
            TaxSetting = new TaxSettingRepository(context);
            FiscalPeriod = new FiscalPeriodRepository(context);
            CurrencySetting = new CurrencySettingRepository(context);
            FiscalPeriodSetting = new FiscalPeriodSettingRepository(context);
            AccountingSetting = new AccountingSettingRepository(context);
            GeneralSetting = new GeneralSettingRepository(context);
        }

        public IGenericRepository<TEntity, int> GetRepository<TEntity>() where TEntity : BaseEntity<int>
        {
            if (!_repositories.TryGetValue(typeof(TEntity), out var repository))
            {
                repository = new GenericRepository<TEntity, int>(_context);
                _repositories[typeof(TEntity)] = repository;
            }

            return (IGenericRepository<TEntity, int>)repository;
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void CreateTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction?.Rollback();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
