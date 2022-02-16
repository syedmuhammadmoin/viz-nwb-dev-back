using System;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Context;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class Level3Repository : GenericRepository<Level3, Guid>, ILevel3Repository
    {
        public Level3Repository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
