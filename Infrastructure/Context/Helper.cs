using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
    public class Helper
    {

        public static void DataConfiguration(ModelBuilder modelBuilder)
        {
            //BankStmt  
            modelBuilder.Entity<BankStmtLines>()
            .HasOne(tc => tc.BankStmtMaster)
            .WithMany(c => c.BankStmtLines)
            .OnDelete(DeleteBehavior.Cascade);

            //JournalEntry
            modelBuilder.Entity<JournalEntryLines>()
            .HasOne(tc => tc.JournalEntryMaster)
            .WithMany(c => c.JournalEntryLines)
            .OnDelete(DeleteBehavior.Cascade);

            //Invoice
            modelBuilder.Entity<InvoiceLines>()
            .HasOne(tc => tc.InvoiceMaster)
            .WithMany(c => c.InvoiceLines)
            .OnDelete(DeleteBehavior.Cascade);

            //Bill
            modelBuilder.Entity<BillLines>()
            .HasOne(tc => tc.BillMaster)
            .WithMany(c => c.BillLines)
            .OnDelete(DeleteBehavior.Cascade);

            //CreditNote
            modelBuilder.Entity<CreditNoteLines>()
            .HasOne(tc => tc.CreditNoteMaster)
            .WithMany(c => c.CreditNoteLines)
            .OnDelete(DeleteBehavior.Cascade);

            //DebitNote
            modelBuilder.Entity<DebitNoteLines>()
            .HasOne(tc => tc.DebitNoteMaster)
            .WithMany(c => c.DebitNoteLines)
            .OnDelete(DeleteBehavior.Cascade);

            //Workflow
            modelBuilder.Entity<WorkFlowTransition>()
            .HasOne(tc => tc.WorkflowMaster)
            .WithMany(c => c.WorkflowTransitions)
            .OnDelete(DeleteBehavior.Cascade);

            //Budget
            modelBuilder.Entity<BudgetLines>()
            .HasOne(tc => tc.BudgetMaster)
            .WithMany(c => c.BudgetLines)
            .OnDelete(DeleteBehavior.Cascade);

            //EstimatedBudget
            modelBuilder.Entity<EstimatedBudgetLines>()
            .HasOne(tc => tc.EstimatedBudgetMaster)
            .WithMany(c => c.EstimatedBudgetLines)
            .OnDelete(DeleteBehavior.Cascade);

            //Purchase Order
            modelBuilder.Entity<PurchaseOrderLines>()
            .HasOne(tc => tc.PurchaseOrderMaster)
            .WithMany(c => c.PurchaseOrderLines)
            .OnDelete(DeleteBehavior.Cascade);

            //Requisition
            modelBuilder.Entity<RequisitionLines>()
            .HasOne(tc => tc.RequisitionMaster)
            .WithMany(c => c.RequisitionLines)
            .OnDelete(DeleteBehavior.Cascade);

            //GRN
            modelBuilder.Entity<GRNLines>()
            .HasOne(tc => tc.GRNMaster)
            .WithMany(c => c.GRNLines)
            .OnDelete(DeleteBehavior.Cascade);

            //PayrollTransaction
            modelBuilder.Entity<PayrollTransactionLines>()
            .HasOne(tc => tc.PayrollTransactionMaster)
            .WithMany(c => c.PayrollTransactionLines)
            .OnDelete(DeleteBehavior.Cascade);

            //Issuance
            modelBuilder.Entity<IssuanceLines>()
            .HasOne(tc => tc.IssuanceMaster)
            .WithMany(c => c.IssuanceLines)
            .OnDelete(DeleteBehavior.Cascade);

            //GoodsReturnNote
            modelBuilder.Entity<GoodsReturnNoteLines>()
            .HasOne(tc => tc.GoodsReturnNoteMaster)
            .WithMany(c => c.GoodsReturnNoteLines)
            .OnDelete(DeleteBehavior.Cascade);

            //IssuanceReturn
            modelBuilder.Entity<IssuanceReturnLines>()
            .HasOne(tc => tc.IssuanceReturnMaster)
            .WithMany(c => c.IssuanceReturnLines)
            .OnDelete(DeleteBehavior.Cascade);

            //Request
            modelBuilder.Entity<RequestLines>()
            .HasOne(tc => tc.RequestMaster)
            .WithMany(c => c.RequestLines)
            .OnDelete(DeleteBehavior.Cascade);

            //Bid Evaluation
            modelBuilder.Entity<BidEvaluationLines>()
            .HasOne(tc => tc.BidEvaluationMaster)
            .WithMany(c => c.BidEvaluationLines)
            .OnDelete(DeleteBehavior.Cascade);

            //Quotation
            modelBuilder.Entity<QuotationLines>()
            .HasOne(tc => tc.QuotationMaster)
            .WithMany(c => c.QuotationLines)
            .OnDelete(DeleteBehavior.Cascade);

            //CallForQuotation
            modelBuilder.Entity<CallForQuotationLines>()
            .HasOne(tc => tc.CallForQuotation)
            .WithMany(c => c.CallForQuotationLines)
            .OnDelete(DeleteBehavior.Cascade);

            //BudgetReappropriation
            modelBuilder.Entity<BudgetReappropriationLines>()
            .HasOne(tc => tc.BudgetReappropriationMaster)
            .WithMany(c => c.BudgetReappropriationLines)
            .OnDelete(DeleteBehavior.Cascade);

            //DepreciationAdjustment
            modelBuilder.Entity<DepreciationAdjustmentLines>()
            .HasOne(tc => tc.Master)
            .WithMany(c => c.DepreciationAdjustmentLines)
            .OnDelete(DeleteBehavior.Cascade);

            //Program
            modelBuilder.Entity<ProgramSemesterCourse>()
            .HasOne(tc => tc.Program)
            .WithMany(c => c.SemesterCourseList)
            .OnDelete(DeleteBehavior.Cascade);

            //Batch
            modelBuilder.Entity<BatchLines>()
            .HasOne(tc => tc.BatchMaster)
            .WithMany(c => c.BatchLines)
            .OnDelete(DeleteBehavior.Cascade);


            //Applicant
            modelBuilder.Entity<ApplicantQualification>()
            .HasOne(tc => tc.Applicant)
            .WithMany(c => c.Qualifications)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicantRelative>()
            .HasOne(tc => tc.Applicant)
            .WithMany(c => c.Relatives)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProgramChallanTemplateLines>()
            .HasOne(tc => tc.ProgramChallanTemplateMaster)
            .WithMany(c => c.ProgramChallanTemplateLines)
            .OnDelete(DeleteBehavior.Cascade);


            //Composite key for Same payroll
            modelBuilder.Entity<PayrollTransactionMaster>()
            .HasAlternateKey(p => new { p.Month, p.Year, p.EmployeeId });

            //Unique Code for all Level 4accounts
            modelBuilder.Entity<Level4>().
                HasIndex(b => b.Code)
                .IsUnique();

            //Unique CNIC for Employee
            modelBuilder.Entity<Employee>().
                HasIndex(b => b.CNIC)
                .IsUnique();

            //Unique Code for all Bank 4accounts
            modelBuilder.Entity<BankAccount>().
                HasIndex(b => b.AccountNumber)
                .IsUnique();

            //Changing Identity users and roles tables name
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable(name: "Users");
            });
            modelBuilder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Roles");
            });
            modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });
            modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });
            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });
            modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });
            modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });

            // Removing IdentityId for assistance in integeration

            modelBuilder.Entity<Department>()
                .Property(et => et.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<Designation>()
                .Property(et => et.Id)
                .ValueGeneratedNever();

            modelBuilder.Entity<Campus>()
                .Property(et => et.Id)
                .ValueGeneratedNever();

        }

        public static string GetCurrentUser(IHttpContextAccessor httpContextAccessor)
        {

            var httpContext = httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                var authenticatedUserName = httpContext.User.Identity.Name;
                return authenticatedUserName;
            }
            return "N/A";
        }
    }
}
