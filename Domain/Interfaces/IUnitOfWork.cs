using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IClientRepository Client { get; }
        IOrganizationRepository Organization { get; }
        IDepartmentRepository Department { get; }
        ILocationRepository Location { get; }
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

        Task SaveAsync();
        void CreateTransaction();
        void Commit();
        void Rollback();
        void Dispose();
    }
}
