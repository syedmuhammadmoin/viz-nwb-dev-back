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

        Task SaveAsync();
        void CreateTransaction();
        void Commit();
        void Rollback();
        void Dispose();
    }
}
