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

        Task SaveAsync();
        void CreateTransaction();
        void Commit();
        void Rollback();
        void Dispose();
    }
}
