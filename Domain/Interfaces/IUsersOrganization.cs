using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUsersOrganization : IGenericRepository<UsersOrganization, int>
    {
        public Task AddRange(List<UsersOrganization> list);
    }
}