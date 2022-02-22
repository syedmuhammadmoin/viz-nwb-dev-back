using Domain.Interfaces;
using Infrastructure.Context;
using Infrastructure.Repositories;
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
        public IClientRepository Client { get; private set; }
        public ICategoryRepository Category { get; private set; }
        public IOrganizationRepository Organization { get; private set; }
        public IDepartmentRepository Department { get; private set; }
        public IWarehouseRepository Warehouse { get; private set; }
        public ILocationRepository Location { get; private set; }

        public IBusinessPartnerRepository BusinessPartner { get; private set; }
        public IProductRepository Product { get; private set; }
        public ILevel4Repository Level4 { get; private set; }

        public ILevel1Repository Level1 { get; private set; }
        public ILevel2Repository Level2 { get; private set; }

        public ILevel3Repository Level3 { get; private set; }

        public IJournalEntryRepository JournalEntry { get; private set; }
        public IInvoiceRepository Invoice { get; private set; }
        public IBillRepository Bill{ get; private set; }
        public ICreditNoteRepository CreditNote { get; private set; }
        public IDebitNoteRepository DebitNote { get; private set; }
        public IPaymentRepository Payment { get; private set; }

        public ITransactionRepository Transaction { get; private set; }
        public ILedgerRepository Ledger { get; private set; }


        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Client = new ClientRepository(context);
            Organization = new OrganizationRepository(context);
            Department = new DepartmentRepository(context);
            Warehouse = new WarehouseRepository(context);
            Location = new LocationRepository(context);
            Category = new CategoryRepository(context);
            BusinessPartner = new BusinessPartnerRepository(context);
            Level4 = new Level4Repository(context);
            Product = new ProductRepository(context);
            JournalEntry = new JournalEntryRepository(context);
            Invoice = new InvoiceRepository(context);
            Bill = new BillRepository(context);
            CreditNote = new CreditNoteRepository(context);
            DebitNote = new DebitNoteRepository(context);
            Payment = new PaymentRepository(context);   
            Transaction = new TransactionRepository(context);
            Ledger = new LedgerRepository(context);
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
            _transaction.Rollback();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
