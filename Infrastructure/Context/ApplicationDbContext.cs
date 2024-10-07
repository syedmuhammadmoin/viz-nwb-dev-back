using Application.Interfaces;
using Domain.Base;
using Domain.Contracts;
using Domain.Entities;
using Infrastructure.Seeds;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace Infrastructure.Context
{
    public class ApplicationDbContext : IdentityDbContext<User, Role,string>, IApplicationDbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Level1> Level1 { get; set; }
        public DbSet<Level2> Level2 { get; set; }
        public DbSet<Level3> Level3 { get; set; }
        public DbSet<Level4> Level4 { get; set; }
        public DbSet<BusinessPartner> BusinessPartners { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CashAccount> CashAccounts { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
        public DbSet<RecordLedger> RecordLedger { get; set; }
        public DbSet<BankStmtMaster> BankStmtMaster { get; set; }
        public DbSet<BankStmtLines> BankStmtLines { get; set; }
        public DbSet<WorkFlowStatus> WorkFlowStatus { get; set; }
        public DbSet<Campus> Campuses { get; set; }
        public DbSet<JournalEntryMaster> JournalEntryMaster { get; set; }
        public DbSet<JournalEntryLines> JournalEntryLines { get; set; }
        public DbSet<PettyCashMaster> PettyCashMaster { get; set; }
        public DbSet<PettyCashLines> PettyCashLines { get; set; }
        public DbSet<InvoiceMaster> InvoiceMaster { get; set; }
        public DbSet<InvoiceLines> InvoiceLines { get; set; }
        public DbSet<BillMaster> BillMaster { get; set; }
        public DbSet<BillLines> BillLines { get; set; }
        public DbSet<CreditNoteMaster> CreditNoteMaster { get; set; }
        public DbSet<CreditNoteLines> CreditNoteLines { get; set; }
        public DbSet<DebitNoteMaster> DebitNoteMaster { get; set; }
        public DbSet<DebitNoteLines> DebitNoteLines { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<BankReconciliation> BankReconciliations { get; set; }
        public DbSet<WorkFlowMaster> WorkFlowMaster { get; set; }
        public DbSet<WorkFlowTransition> WorkFlowTransitions { get; set; }
        public DbSet<TransactionReconcile> TransactionReconciles { get; set; }
        public DbSet<BudgetMaster> BudgetMaster { get; set; }
        public DbSet<BudgetLines> BudgetLines { get; set; }
        public DbSet<EstimatedBudgetMaster> EstimatedBudgetMaster { get; set; }
        public DbSet<EstimatedBudgetLines> EstimatedBudgetLines { get; set; }
        public DbSet<PurchaseOrderMaster> PurchaseOrderMaster { get; set; }
        public DbSet<PurchaseOrderLines> PurchaseOrderLines { get; set; }
        public DbSet<RequisitionMaster> RequisitionMaster { get; set; }
        public DbSet<RequisitionLines> RequisitionLines { get; set; }
        public DbSet<GRNMaster> GRNMaster { get; set; }
        public DbSet<GRNLines> GRNLines { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<PayrollItem> PayrollItems { get; set; }
        public DbSet<PayrollItemEmployee> PayrollItemEmployees { get; set; }
        public DbSet<PayrollTransactionMaster> PayrollTransactionMaster { get; set; }
        public DbSet<PayrollTransactionLines> PayrollTransactionLines { get; set; }
        public DbSet<Taxes> Taxes { get; set; }
        public DbSet<UnitOfMeasurement> UnitOfMeasurement { get; set; }
        public DbSet<IssuanceMaster> IssuanceMaster { get; set; }
        public DbSet<IssuanceLines> IssuanceLines { get; set; }
        public DbSet<FileUpload> FileUpload { get; set; }
        public DbSet<Stock> Stock { get; set; }
        public DbSet<POToGRNLineReconcile> POToGRNLineReconcile { get; set; }
        public DbSet<RequisitionToIssuanceLineReconcile> RequisitionToIssuanceLineReconcile { get; set; }
        public DbSet<GoodsReturnNoteMaster> GoodsReturnNoteMaster { get; set; }
        public DbSet<GoodsReturnNoteLines> GoodsReturnNoteLines { get; set; }
        public DbSet<GRNToGoodsReturnNoteLineReconcile> GRNToGoodsReturnNoteLineReconcile { get; set; }
        public DbSet<IssuanceToIssuanceReturnLineReconcile> IssuanceToIssuanceReturnLineReconcile { get; set; }
        public DbSet<IssuanceReturnMaster> IssuanceReturnMaster { get; set; }
        public DbSet<IssuanceReturnLines> IssuanceReturnLines { get; set; }
        public DbSet<Remark> Remarks { get; set; }
        public DbSet<LogItem> LogItems { get; set; }
        public DbSet<RequestMaster> RequestMaster { get; set; }
        public DbSet<RequestLines> RequestLines { get; set; }
        public DbSet<BidEvaluationMaster> BidEvaluationMaster { get; set; }
        public DbSet<BidEvaluationLines> BidEvaluationLines { get; set; }
        public DbSet<QuotationMaster> QuotationMaster { get; set; }
        public DbSet<QuotationLines> QuotationLines { get; set; }
        public DbSet<CallForQuotationMaster> CallForQuotationMaster { get; set; }
        public DbSet<CallForQuotationLines> CallForQuotationLines { get; set; }
        public DbSet<QuotationComparativeMaster> QuotationComparativeMaster { get; set; }
        public DbSet<DepreciationModel> DepreciationModels { get; set; }
        public DbSet<FixedAsset> FixedAssets { get; set; }
        public DbSet<CWIP> CWIPs { get; set; }
        public DbSet<Disposal> Disposals { get; set; }
        public DbSet<BudgetReappropriationMaster> BudgetReappropriationMaster { get; set; }
        public DbSet<BudgetReappropriationLines> BudgetReappropriationLines { get; set; }
        public DbSet<DepreciationAdjustmentMaster> DepreciationAdjustmentMaster { get; set; }
        public DbSet<DepreciationAdjustmentLines> DepreciationAdjustmentLines { get; set; }
        public DbSet<DepreciationRegister> DepreciationRegister { get; set; }
        public DbSet<FixedAssetLines> FixedAssetLines { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<AcademicDepartment> AcademicDepartments { get; set; }
        public DbSet<Degree> Degrees { get; set; }
        public DbSet<Program> Programs { get; set; }
        public DbSet<ProgramSemesterCourse> ProgramSemesterCourses { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Qualification> Qualifications { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<FeeItem> FeeItems { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Domicile> Domiciles { get; set; }
        public DbSet<InviteUser> InviteUser { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<BatchMaster> BatchMaster { get; set; }
        public DbSet<BatchLines> BatchLines { get; set; }
        public DbSet<AdmissionCriteria> AdmissionCriteria { get; set; }
        public DbSet<BatchAdmissionCriteria> BatchAdmissionCriteria { get; set; }
        public DbSet<Applicant> Applicants { get; set; }
        public DbSet<ApplicantQualification> ApplicantQualifications { get; set; }
        public DbSet<ApplicantRelative> ApplicantRelatives { get; set; }
        public DbSet<AdmissionApplication> AdmissionApplications { get; set; }
        public DbSet<AdmissionApplicationHistory> AdmissionApplicationHistories { get; set; }
        public DbSet<ProgramChallanTemplateMaster> ProgramChallanTemplateMaster { get; set; }
        public DbSet<ProgramChallanTemplateLines> ProgramChallanTemplateLines { get; set; }
        public DbSet<UsersOrganization> UsersOrganization { get; set; }
        public DbSet<Journal> Journals { get; set; }
        public DbSet<Currency> Currency { get; set; }
        
        public DbSet<TaxGroup> TaxGroups {  get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //changing cascade delete behavior
            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.SetQueryFilterOnAllEntities<IMustHaveTenant>(p => p.OrganizationId == GetOrganizationId());

          
            Helper.DataConfiguration(modelBuilder);
            Seeding.seeds(modelBuilder);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if (entry.Entity is BaseEntity trackable)
                {
                    var now = DateTime.UtcNow;
                    var user = Helper.GetCurrentUser(_httpContextAccessor);
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            trackable.ModifiedDate = now;
                            trackable.ModifiedBy = user;
                            break;

                        case EntityState.Added:
                            trackable.CreatedDate = now;
                            trackable.CreatedBy = user;
                            trackable.ModifiedDate = now;
                            trackable.ModifiedBy = user;
                            break;
                    }
                }
                if (entry.Entity is IMustHaveTenant trackables)
                {
                    int orgId = GetOrganizationId();
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                        case EntityState.Added:
                            trackables.OrganizationId = trackables.OrganizationId != 0 ? trackables.OrganizationId : orgId;
                            break;
                    }
                }
            }
        }
        private int GetOrganizationId()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                if (httpContext.User.Claims.Any())
                {
                    var orgId = httpContext.User.Claims.FirstOrDefault(i => i.Type == "Organization").Value;
                    return Int32.Parse(orgId);
                }
            }
            return 0;
        }
    }
}
