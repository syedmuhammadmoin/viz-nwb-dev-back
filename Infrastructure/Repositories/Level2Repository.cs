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
    public class Level2Repository : GenericRepository<Level2, Guid>, ILevel2Repository
    {
        public Level2Repository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
