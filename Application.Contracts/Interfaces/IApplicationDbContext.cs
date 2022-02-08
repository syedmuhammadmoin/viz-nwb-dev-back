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
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
