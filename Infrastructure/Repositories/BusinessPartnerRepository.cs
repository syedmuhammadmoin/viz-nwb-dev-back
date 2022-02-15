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
    public class BusinessPartnerRepository : GenericRepository<BusinessPartner, int>, IBusinessPartnerRepository
    {
        public BusinessPartnerRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
