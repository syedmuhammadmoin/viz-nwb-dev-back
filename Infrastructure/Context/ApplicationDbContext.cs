using Application.Interfaces;
using Domain.Entities;
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
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Level1> Level1 { get; set; }
        public DbSet<Level2> Level2 { get; set; }
        public DbSet<Level3> Level3 { get; set; }
        public DbSet<Level4> Level4 { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BusinessPartner> BusinessPartners { get; set; }
        public DbSet<Product> Products { get; set; }
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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            //changing cascade delete behavior
            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
            
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
        }
    }
}
