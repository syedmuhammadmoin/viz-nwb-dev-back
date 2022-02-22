using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Level4> Level4 { get; set; }
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
        public DbSet<Payment> Payments { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
