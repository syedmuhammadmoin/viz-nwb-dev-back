using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class Level4Repository : GenericRepository<Level4, Guid>, ILevel4Repository
    {
        public Level4Repository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
