using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class Level1Repository : GenericRepository<Level1, Guid>, ILevel1Repository
    {
        public Level1Repository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
