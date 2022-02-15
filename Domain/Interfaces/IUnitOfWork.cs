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
        ICategoryRepository Category { get; }
        IBusinessPartnerRepository BusinessPartner { get; }
        ILevel4Repository Level4 { get; }
        IProductRepository Product { get; }

        Task SaveAsync();
        void CreateTransaction();
        void Commit();
        void Rollback();
        void Dispose();
    }
}
