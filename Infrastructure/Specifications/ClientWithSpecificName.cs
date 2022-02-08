using Domain.Entities;
using Infrastructure.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Specifications
{
    public class ClientWithSpecificName : BaseSpecification<Client>
    {
        public ClientWithSpecificName(string name) : base(x => x.Name.Contains(name))
        {

        }
        public ClientWithSpecificName()
        {

        }
    }
}
