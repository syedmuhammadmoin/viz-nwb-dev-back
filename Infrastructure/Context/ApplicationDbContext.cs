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

            //Composite key for Same payroll
            modelBuilder.Entity<PayrollTransactionMaster>()
            .HasAlternateKey(p => new { p.Month, p.Year, p.EmployeeId });

            //Unique Code for all Level 4accounts
            modelBuilder.Entity<Level4>().
                HasIndex(b => b.Code)
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
                Code = "F"
            },
             new Level1
             {
                 Id = new Guid("20000000-5566-7788-99AA-BBCCDDEEFF00"),
                 Name = "Liability",
                 Code = "G"
             },
             new Level1
             {
                 Id = new Guid("30000000-5566-7788-99AA-BBCCDDEEFF00"),
                 Name = "Accumulated Fund",
                 Code = "P"
             },
             new Level1
             {
                 Id = new Guid("40000000-5566-7788-99AA-BBCCDDEEFF00"),
                 Name = "Revenue",
                 Code = "C"
             },
             new Level1
             {
                 Id = new Guid("50000000-5566-7788-99AA-BBCCDDEEFF00"),
                 Name = "Expenses",
                 Code = "A"
             });

            //Adding in Level 2
            modelBuilder.Entity<Level2>().HasData(new Level2
            {
                Id = new Guid("11000000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Non - Current Assets",
                Level1_id = new Guid("10000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "F03"
            },
            new Level2
            {
                Id = new Guid("12000000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Current Assets",
                Level1_id = new Guid("10000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "F02"
            },
            new Level2
            {
                Id = new Guid("21000000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Non - Current Liabilities",
                Level1_id = new Guid("20000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "G02"
            },
            new Level2
            {
                Id = new Guid("22000000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Current Liabilities",
                Level1_id = new Guid("20000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "G01"
            },
            new Level2
            {
                Id = new Guid("31000000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Grants",
                Level1_id = new Guid("30000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "P02"
            }, new Level2
            {
                Id = new Guid("32000000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Surplus/(Deficit)",
                Level1_id = new Guid("30000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "P01"
            },
            new Level2
            {
                Id = new Guid("41000000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Onsite And Offsite Revenue",
                Level1_id = new Guid("40000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "C02"
            },
            new Level2
            {
                Id = new Guid("51000000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Employee Related Expenses",
                Level1_id = new Guid("50000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "A01"
            },
            new Level2
            {
                Id = new Guid("52000000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Project Pre-Investment Analysis",
                Level1_id = new Guid("50000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "A02"
            },
            new Level2
            {
                Id = new Guid("53000000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Operating Expenses",
                Level1_id = new Guid("50000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "A03"
            },
            new Level2
            {
                Id = new Guid("54000000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Employees Retirement Benefits",
                Level1_id = new Guid("50000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "A04"
            },
            new Level2
            {
                Id = new Guid("55000000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Transfers",
                Level1_id = new Guid("50000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "A06"
            }
            //new Level2
            //{
            //    Id = new Guid("56000000-5566-7788-99AA-BBCCDDEEFF00"),
            //    Name = "Repair And Maintenance",
            //    Level1_id = new Guid("50000000-5566-7788-99AA-BBCCDDEEFF00"),
            //    Code = "A13"
            //}
            );

            //Adding in Level 3
            modelBuilder.Entity<Level3>().HasData(new Level3
            {
                Id = new Guid("11100000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Property Plant And Equipment",
                Level2_id = new Guid("11000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "F031"
            },
            new Level3
            {
                Id = new Guid("11200000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Capital Work-In-Progress",
                Level2_id = new Guid("11000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "F032"
            }, new Level3
            {
                Id = new Guid("11300000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Intangible Assets",
                Level2_id = new Guid("11000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "F033"
            }, new Level3
            {
                Id = new Guid("11400000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Intangible Asset Under Development",
                Level2_id = new Guid("11000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "F034"
            },
            new Level3
            {
                Id = new Guid("11500000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Long Term Loan",
                Level2_id = new Guid("11000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "F035"
            }, new Level3
            {
                Id = new Guid("11600000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Investment Property",
                Level2_id = new Guid("11000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "F036"
            }, new Level3
            {
                Id = new Guid("11700000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Long Term Investment",
                Level2_id = new Guid("11000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "F037"
            }, new Level3
            {
                Id = new Guid("11800000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Advance To Employees",
                Level2_id = new Guid("11000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "F038"
            }
            , new Level3
            {
                Id = new Guid("12100000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Other Receivables",
                Level2_id = new Guid("12000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "F021"
            }, new Level3
            {
                Id = new Guid("12200000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Receivable From Government Authorities",
                Level2_id = new Guid("12000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "F022"
            }, new Level3
            {
                Id = new Guid("12300000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Affiliated Colleges Fee Receivable",
                Level2_id = new Guid("12000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "F023"
            }, new Level3
            {
                Id = new Guid("12400000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Inventory",
                Level2_id = new Guid("12000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "F024"
            }, new Level3
            {
                Id = new Guid("12500000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Cash balances with Banks ",
                Level2_id = new Guid("12000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "F025"
            }, new Level3
            {
                Id = new Guid("12600000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Petty Cash",
                Level2_id = new Guid("12000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "F026"
            }, new Level3
            {
                Id = new Guid("12700000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Advances, Prepayments & Deposits",
                Level2_id = new Guid("12000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "F027"
            }, new Level3
            {
                Id = new Guid("12800000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Short Term Investments",
                Level2_id = new Guid("12000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "F028"
            }, new Level3
            {
                Id = new Guid("12900000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Receivable From Students",
                Level2_id = new Guid("12000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "F029"
            }, new Level3
            {
                Id = new Guid("12110000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Rent Receivable",
                Level2_id = new Guid("12000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "F0210"
            }, new Level3
            {
                Id = new Guid("12120000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Interest Receivable",
                Level2_id = new Guid("12000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "F0211"
            }
            , new Level3
            {
                Id = new Guid("21100000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Long Term Borrowings",
                Level2_id = new Guid("21000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "G021"
            }, new Level3
            {
                Id = new Guid("21200000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Post Retirement Benefit Plan",
                Level2_id = new Guid("21000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "G022"
            }, new Level3
            {
                Id = new Guid("21300000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Long Term Compensated Absences",
                Level2_id = new Guid("21000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "G023"
            }
            , new Level3
            {
                Id = new Guid("21400000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Security Deposit-Non Current",
                Level2_id = new Guid("21000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "G024"
            }, new Level3
            {
                Id = new Guid("21500000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Deferred Capital Grant",
                Level2_id = new Guid("21000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "G025"
            }, new Level3
            {
                Id = new Guid("21600000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Others",
                Level2_id = new Guid("21000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "G026"
            }
            , new Level3
            {
                Id = new Guid("22100000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Accounts Payable",
                Level2_id = new Guid("22000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "G01"
            }, new Level3
            {
                Id = new Guid("22200000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Short Term Borrowings",
                Level2_id = new Guid("22000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "G02"
            }, new Level3
            {
                Id = new Guid("22300000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Other Liabilities",
                Level2_id = new Guid("22000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "G03"
            }, new Level3
            {
                Id = new Guid("22400000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Security Deposit-Short Term",
                Level2_id = new Guid("22000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "G04"
            }, new Level3
            {
                Id = new Guid("22500000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Financial Assistance/ Scholarships",
                Level2_id = new Guid("22000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "G05"
            }
            , new Level3
            {
                Id = new Guid("31100000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Federal Govt Grant",
                Level2_id = new Guid("31000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "P021"
            }, new Level3
            {
                Id = new Guid("31200000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Sindh Govt Grant",
                Level2_id = new Guid("31000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "P022"
            }
            , new Level3
            {
                Id = new Guid("32100000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Surplus/(Deficit) Of Comprehensive Income",
                Level2_id = new Guid("32000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "P011"
            }, new Level3
            {
                Id = new Guid("32200000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Retained Earning",
                Level2_id = new Guid("32000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "P012"
            }
            , new Level3
            {
                Id = new Guid("41100000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Education General Fees ",
                Level2_id = new Guid("41000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "CO21"
            }
            , new Level3
            {
                Id = new Guid("41200000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Hostel Fees / User Charges ",
                Level2_id = new Guid("41000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "CO22"
            }, new Level3
            {
                Id = new Guid("41300000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Income From Endowments",
                Level2_id = new Guid("41000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "CO23"
            }, new Level3
            {
                Id = new Guid("41400000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Income From Services Rendered ",
                Level2_id = new Guid("41000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "CO24"
            }
            , new Level3
            {
                Id = new Guid("41500000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Income From Intellectual Property ",
                Level2_id = new Guid("41000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "CO25"
            }, new Level3
            {
                Id = new Guid("41600000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Others",
                Level2_id = new Guid("41000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "CO26"
            }, new Level3
            {
                Id = new Guid("41700000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Grant Revenue",
                Level2_id = new Guid("41000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "CO27"
            }
            , new Level3
            {
                Id = new Guid("51100000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Pay",
                Level2_id = new Guid("51000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "A011"
            }, new Level3
            {
                Id = new Guid("51200000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Allowances",
                Level2_id = new Guid("51000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "A012"
            }
            , new Level3
            {
                Id = new Guid("52100000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Feasibility Studies",
                Level2_id = new Guid("52000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "A021"
            }, new Level3
            {
                Id = new Guid("52200000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Research Survey & Exploratory Operations",
                Level2_id = new Guid("52000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "A022"
            }
            , new Level3
            {
                Id = new Guid("53100000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Fees",
                Level2_id = new Guid("53000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "A031"
            }, new Level3
            {
                Id = new Guid("53200000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Communication Expense",
                Level2_id = new Guid("53000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "A032"
            }, new Level3
            {
                Id = new Guid("53300000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Utilities Expense",
                Level2_id = new Guid("53000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "A033"
            }, new Level3
            {
                Id = new Guid("53400000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Occupancy Cost",
                Level2_id = new Guid("53000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "A034"
            }, new Level3
            {
                Id = new Guid("53500000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Operating Leases",
                Level2_id = new Guid("53000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "A035"
            }, new Level3
            {
                Id = new Guid("53600000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Motor Vehicles",
                Level2_id = new Guid("53000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "A036"
            }, new Level3
            {
                Id = new Guid("53700000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Consultancy & Contractual Work",
                Level2_id = new Guid("53000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "A037"
            }, new Level3
            {
                Id = new Guid("53800000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Travel & Transportation",
                Level2_id = new Guid("53000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "A038"
            }, new Level3
            {
                Id = new Guid("53900000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "General ",
                Level2_id = new Guid("53000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "A039"
            }
            , new Level3
            {
                Id = new Guid("54100000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Pension",
                Level2_id = new Guid("54000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "A041"
            }
            , new Level3
            {
                Id = new Guid("55100000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Scholarships",
                Level2_id = new Guid("55000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "A061"
            }, new Level3
            {
                Id = new Guid("55200000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Technical Assistance",
                Level2_id = new Guid("55000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "A062"
            }, new Level3
            {
                Id = new Guid("55300000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Entertainment & Gifts",
                Level2_id = new Guid("55000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "A063"
            }, new Level3
            {
                Id = new Guid("55400000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Other Transfer Payments",
                Level2_id = new Guid("55000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "A064"
            }, new Level3
            {
                Id = new Guid("53110000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Repair And Maintenance",
                Level2_id = new Guid("53000000-5566-7788-99AA-BBCCDDEEFF00"),
                Code = "A040"
            }
            //, new Level3
            //{
            //    Id = new Guid("56100000-5566-7788-99AA-BBCCDDEEFF00"),
            //    Name = "Transport",
            //    Level2_id = new Guid("56000000-5566-7788-99AA-BBCCDDEEFF00"),
            //    Code = "A130"
            //}, new Level3
            //{
            //    Id = new Guid("56200000-5566-7788-99AA-BBCCDDEEFF00"),
            //    Name = "Machinary & Equipment",
            //    Level2_id = new Guid("56000000-5566-7788-99AA-BBCCDDEEFF00"),
            //    Code = "A131"
            //}, new Level3
            //{
            //    Id = new Guid("56300000-5566-7788-99AA-BBCCDDEEFF00"),
            //    Name = "Furniture & Fixture",
            //    Level2_id = new Guid("56000000-5566-7788-99AA-BBCCDDEEFF00"),
            //    Code = "A132"
            //}, new Level3
            //{
            //    Id = new Guid("56400000-5566-7788-99AA-BBCCDDEEFF00"),
            //    Name = "Building & Structure",
            //    Level2_id = new Guid("56000000-5566-7788-99AA-BBCCDDEEFF00"),
            //    Code = "A133"
            //}, new Level3
            //{
            //    Id = new Guid("56500000-5566-7788-99AA-BBCCDDEEFF00"),
            //    Name = "Computer Equipments",
            //    Level2_id = new Guid("56000000-5566-7788-99AA-BBCCDDEEFF00"),
            //    Code = "A137"
            //}, new Level3
            //{
            //    Id = new Guid("56600000-5566-7788-99AA-BBCCDDEEFF00"),
            //    Name = "Generals",
            //    Level2_id = new Guid("56000000-5566-7788-99AA-BBCCDDEEFF00"),
            //    Code = "A138"
            //}, new Level3
            //{
            //    Id = new Guid("56700000-5566-7788-99AA-BBCCDDEEFF00"),
            //    Name = "Depreciation, Amortization & Impairment",
            //    Level2_id = new Guid("56000000-5566-7788-99AA-BBCCDDEEFF00"),
            //    Code = "A180"
            //}, new Level3
            //{
            //    Id = new Guid("56800000-5566-7788-99AA-BBCCDDEEFF00"),
            //    Name = "Bad Debts",
            //    Level2_id = new Guid("56000000-5566-7788-99AA-BBCCDDEEFF00"),
            //    Code = "A181"
            //}, new Level3
            //{
            //    Id = new Guid("56900000-5566-7788-99AA-BBCCDDEEFF00"),
            //    Name = "Unrealized Losses",
            //    Level2_id = new Guid("56000000-5566-7788-99AA-BBCCDDEEFF00"),
            //    Code = "A182"
            //}
            );

            //Adding in Level 4
            modelBuilder.Entity<Level4>().HasData( new Level4
            {
                Id = new Guid("32110000-5566-7788-99AA-BBCCDDEEFF00"),
                Name = "Opening Balance equity",
                Code = "P01101",
                AccountType = AccountType.SystemDefined,
                Level3_id = new Guid("32100000-5566-7788-99AA-BBCCDDEEFF00"),
                Level1_id = new Guid("30000000-5566-7788-99AA-BBCCDDEEFF00")
            });

            //Adding seeds in TaxAccounts
            modelBuilder.Entity<Taxes>()
                .HasData(
                    new Taxes(1, "Sales Tax Asset", TaxType.SalesTaxAsset),
                    new Taxes(2, "Sales Tax Liability", TaxType.SalesTaxLiability),
                    new Taxes(3, "Income Tax Asset", TaxType.IncomeTaxAsset),
                    new Taxes(4, "Income Tax Liability", TaxType.IncomeTaxLiability),
                    new Taxes(5, "SRB Tax Asset", TaxType.SRBTaxAsset),
                    new Taxes(6, "SRB Tax Liability", TaxType.SRBTaxLiability)
                    );
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
