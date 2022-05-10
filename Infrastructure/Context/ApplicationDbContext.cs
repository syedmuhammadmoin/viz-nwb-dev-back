using Application.Interfaces;
using Domain.Base;
using Domain.Constants;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Context
{
    public class ApplicationDbContext : IdentityDbContext<User>, IApplicationDbContext
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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //changing cascade delete behavior
            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }

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


            //Adding seeds in organization table
            modelBuilder.Entity<Organization>()
                .HasData(
                    new Organization(1, "SBBU")
                );

            //Adding seeds in workflow status
            modelBuilder.Entity<WorkFlowStatus>()
                .HasData(
                    new WorkFlowStatus(1, "Draft", DocumentStatus.Draft, StatusType.PreDefined),
                    new WorkFlowStatus(2, "Rejected", DocumentStatus.Rejected, StatusType.PreDefinedInList),
                    new WorkFlowStatus(3, "Unpaid", DocumentStatus.Unpaid, StatusType.PreDefined),
                    new WorkFlowStatus(4, "Partial Paid", DocumentStatus.Partial, StatusType.PreDefined),
                    new WorkFlowStatus(5, "Paid", DocumentStatus.Paid, StatusType.PreDefined),
                    new WorkFlowStatus(6, "Submitted", DocumentStatus.Submitted, StatusType.PreDefinedInList),
                    new WorkFlowStatus(7, "Cancelled", DocumentStatus.Cancelled, StatusType.PreDefined)
                );

            //Adding seeds of Chart of Account 
            //Adding in Level 1
            modelBuilder.Entity<Level1>().HasData(new Level1
            {
                Id = new Guid("10000000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Assets",

            },
             new Level1
             {
                 Id = new Guid("20000000-5566-7788-99AA-BBCCDDEEFF00"),
                 Name = "Liability",

             },
             new Level1
             {
                 Id = new Guid("30000000-5566-7788-99AA-BBCCDDEEFF00"),
                 Name = "Equity",

             },
             new Level1
             {
                 Id = new Guid("40000000-5566-7788-99AA-BBCCDDEEFF00"),
                 Name = "Income",

             },
             new Level1
             {
                 Id = new Guid("50000000-5566-7788-99AA-BBCCDDEEFF00"),
                 Name = "Expenses",

             });

            //Adding in Level 2
            modelBuilder.Entity<Level2>().HasData(new Level2
            {
                Id = new Guid("11000000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Non - Current Assets",
                Level1_id = new Guid("10000000-5566-7788-99AA-BBCCDDEEFF00"),
            },
            new Level2
            {
                Id = new Guid("12000000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Current Assets",
                Level1_id = new Guid("10000000-5566-7788-99AA-BBCCDDEEFF00"),
            },
            new Level2
            {
                Id = new Guid("21000000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Non - Current Liabilities",
                Level1_id = new Guid("20000000-5566-7788-99AA-BBCCDDEEFF00"),
            },
            new Level2
            {
                Id = new Guid("22000000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Current Liabilities",
                Level1_id = new Guid("20000000-5566-7788-99AA-BBCCDDEEFF00"),
            },
            new Level2
            {
                Id = new Guid("31000000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Owner's Equity",
                Level1_id = new Guid("30000000-5566-7788-99AA-BBCCDDEEFF00"),
            },
            new Level2
            {
                Id = new Guid("41000000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Income",
                Level1_id = new Guid("40000000-5566-7788-99AA-BBCCDDEEFF00"),
            },
            new Level2
            {
                Id = new Guid("51000000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Cost of Revenue (COGS)",
                Level1_id = new Guid("50000000-5566-7788-99AA-BBCCDDEEFF00"),
            },
            new Level2
            {
                Id = new Guid("52000000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "General & Administrative Expenses",
                Level1_id = new Guid("50000000-5566-7788-99AA-BBCCDDEEFF00"),
            });

            //Adding in Level 3
            modelBuilder.Entity<Level3>().HasData(new Level3
            {
                Id = new Guid("11100000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Fixed Assets",
                Level2_id = new Guid("11000000-5566-7788-99AA-BBCCDDEEFF00"),
            },
            new Level3
            {
                Id = new Guid("11200000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Intangible Assets",
                Level2_id = new Guid("11000000-5566-7788-99AA-BBCCDDEEFF00"),
            }, new Level3
            {
                Id = new Guid("11300000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Financial Assets",
                Level2_id = new Guid("11000000-5566-7788-99AA-BBCCDDEEFF00"),
            }, new Level3
            {
                Id = new Guid("11400000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Leased Assets",
                Level2_id = new Guid("11000000-5566-7788-99AA-BBCCDDEEFF00"),
            }, new Level3
            {
                Id = new Guid("12100000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Cash & Cash Equivalents",
                Level2_id = new Guid("12000000-5566-7788-99AA-BBCCDDEEFF00"),
            }, new Level3
            {
                Id = new Guid("12200000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Accounts Receivable",
                Level2_id = new Guid("12000000-5566-7788-99AA-BBCCDDEEFF00"),
            }, new Level3
            {
                Id = new Guid("12300000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Prepayments, Advances, deposits",
                Level2_id = new Guid("12000000-5566-7788-99AA-BBCCDDEEFF00"),
            }, new Level3
            {
                Id = new Guid("12400000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Inventory / Merchandise",
                Level2_id = new Guid("12000000-5566-7788-99AA-BBCCDDEEFF00"),
            }, new Level3
            {
                Id = new Guid("21100000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Long - term Borrowings",
                Level2_id = new Guid("21000000-5566-7788-99AA-BBCCDDEEFF00"),
            }, new Level3
            {
                Id = new Guid("22100000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Accounts Payable",
                Level2_id = new Guid("22000000-5566-7788-99AA-BBCCDDEEFF00"),
            }, new Level3
            {
                Id = new Guid("22200000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Other Liability",
                Level2_id = new Guid("22000000-5566-7788-99AA-BBCCDDEEFF00"),
            }, new Level3
            {
                Id = new Guid("31100000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Share Capital",
                Level2_id = new Guid("31000000-5566-7788-99AA-BBCCDDEEFF00"),
            }, new Level3
            {
                Id = new Guid("31200000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Reserves",
                Level2_id = new Guid("31000000-5566-7788-99AA-BBCCDDEEFF00"),
            }, new Level3
            {
                Id = new Guid("41100000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Operating Income",
                Level2_id = new Guid("41000000-5566-7788-99AA-BBCCDDEEFF00"),
            }, new Level3
            {
                Id = new Guid("41200000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Non - Operating Income",
                Level2_id = new Guid("41000000-5566-7788-99AA-BBCCDDEEFF00"),
            }, new Level3
            {
                Id = new Guid("41300000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Discount",
                Level2_id = new Guid("41000000-5566-7788-99AA-BBCCDDEEFF00"),
            }, new Level3
            {
                Id = new Guid("51100000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Direct Cost",
                Level2_id = new Guid("51000000-5566-7788-99AA-BBCCDDEEFF00"),
            }, new Level3
            {
                Id = new Guid("52100000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Administrative Expenses",
                Level2_id = new Guid("52000000-5566-7788-99AA-BBCCDDEEFF00"),
            }, new Level3
            {
                Id = new Guid("52200000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Selling, Promotions & Advertising",
                Level2_id = new Guid("52000000-5566-7788-99AA-BBCCDDEEFF00"),
            });

            //Adding in Level 4
            modelBuilder.Entity<Level4>().HasData(new Level4
            {
                Id = new Guid("11110000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Land",
                Level3_id = new Guid("11100000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("10000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("11120000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Buildings",
                Level3_id = new Guid("11100000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("10000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("11130000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Equipment",
                Level3_id = new Guid("11100000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("10000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("11140000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Accumulated Depreciation - Buildings (Contra Asset)",
                Level3_id = new Guid("11100000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("10000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("11410000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Leased Building",
                Level3_id = new Guid("11400000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("10000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("11420000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Leased Equipment",
                Level3_id = new Guid("11400000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("10000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("11430000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Accumulated Depreciation - Leased Building (Contra Asset)",
                Level3_id = new Guid("11400000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("10000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("11440000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Accumulated Depreciation - Leased Equipment (Contra Asset)",
                Level3_id = new Guid("11400000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("10000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("12110000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Bank",
                Level3_id = new Guid("12100000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("10000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("12120000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Cash",
                Level3_id = new Guid("12100000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("10000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("12210000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Trade Receivable",
                Level3_id = new Guid("12200000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("10000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("12220000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Other Receivable",
                Level3_id = new Guid("12200000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("10000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("12310000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Sales Tax Asset",
                Level3_id = new Guid("12300000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("10000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("12320000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Income Tax Asset",
                Level3_id = new Guid("12300000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("10000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("12410000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Raw Material",
                Level3_id = new Guid("12400000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("10000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("12420000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Work in Progress",
                Level3_id = new Guid("12400000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("10000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("12430000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Finished Goods",
                Level3_id = new Guid("12400000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("10000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("12440000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Goods in Transition",
                Level3_id = new Guid("12400000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("10000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("21110000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Long - Term Loans",
                Level3_id = new Guid("21100000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("20000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("21120000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Lease Liability",
                Level3_id = new Guid("21100000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("20000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("22110000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Bank Over draft",
                Level3_id = new Guid("22100000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("20000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("22120000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Trade Accounts Payable",
                Level3_id = new Guid("22100000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("20000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("22130000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Accrued Expenses, Loan & Other Payable",
                Level3_id = new Guid("22100000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("20000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("22140000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Short Term Lease Payable",
                Level3_id = new Guid("22100000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("20000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("22150000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Sales Tax Liability",
                Level3_id = new Guid("22100000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("20000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("22160000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Income Tax Liability",
                Level3_id = new Guid("22100000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("20000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("22210000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Unearned Revenue",
                Level3_id = new Guid("22200000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("20000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("31110000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Paid up Share Capital",
                Level3_id = new Guid("31100000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("30000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("31210000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Share Premium",
                Level3_id = new Guid("31200000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("30000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("31220000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Suplus /Defecit/ Retained Earnings",
                Level3_id = new Guid("31200000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("30000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("31230000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Revaluation Reserve",
                Level3_id = new Guid("31200000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("30000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("31240000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Exchange Differences",
                Level3_id = new Guid("31200000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("30000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("31250000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Drawings",
                Level3_id = new Guid("31200000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("30000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("31260000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Opening Balance equity",
                Level3_id = new Guid("31200000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("30000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("41110000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Revenue",
                Level3_id = new Guid("41100000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("40000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("41210000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Interest Income",
                Level3_id = new Guid("41200000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("40000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("41220000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Gain/Loss on Sale of Assets",
                Level3_id = new Guid("41200000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("40000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("41310000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Discount",
                Level3_id = new Guid("41300000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("40000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("51110000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Direct Labor / Salaries",
                Level3_id = new Guid("51100000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("50000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("51120000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Direct Material",
                Level3_id = new Guid("51100000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("50000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("51130000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Depreciation Expense",
                Level3_id = new Guid("51100000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("50000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("51140000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Cost Of Goods Sold",
                Level3_id = new Guid("51100000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("50000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("52110000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Salaries Expense",
                Level3_id = new Guid("52100000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("50000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("52120000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Wages Expense",
                Level3_id = new Guid("52100000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("50000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("52130000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Utilities Expense",
                Level3_id = new Guid("52100000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("50000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("52140000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Entertainment / Meals Expense",
                Level3_id = new Guid("52100000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("50000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("52150000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Printing & Stationary Expense",
                Level3_id = new Guid("52100000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("50000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("52160000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Rent Expense",
                Level3_id = new Guid("52100000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("50000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("52170000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Office Expense",
                Level3_id = new Guid("52100000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("50000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("52180000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Repair & Maintenance Expense",
                Level3_id = new Guid("52100000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("50000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("52190000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Transportation & Conveyance Expense",
                Level3_id = new Guid("52100000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("50000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("52101000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Interest Expense",
                Level3_id = new Guid("52100000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("50000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("52101100-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Tax Expense",
                Level3_id = new Guid("52100000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("50000000-5566-7788-99AA-BBCCDDEEFF00")
            }, new Level4
            {
                Id = new Guid("52210000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Advertising / Marketing Expense",
                Level3_id = new Guid("52200000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("50000000-5566-7788-99AA-BBCCDDEEFF00")
            });
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
                if (entry.Entity is BaseEntity<int> trackable)
                {
                    var now = DateTime.UtcNow;
                    var user = GetCurrentUser();
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
            }
        }
        private string GetCurrentUser()
        {
            //return "UserName"; // TODO implement your own logic
            // If you are using ASP.NET Core, you should look at this answer on StackOverflow
            // https://stackoverflow.com/a/48554738/2996339
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                var authenticatedUserName = httpContext.User.Identity.Name;
                return authenticatedUserName;
                // If it returns null, even when the user was authenticated, you may try to get the value of a specific claim 
                //var authenticatedUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                // var authenticatedUserId = _httpContextAccessor.HttpContext.User.FindFirst("sub").Value
            }
            return "UserName";
        }
    }
}
