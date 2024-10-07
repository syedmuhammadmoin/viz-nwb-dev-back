using Application.Contracts.Filters;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    internal interface ISpecificationFactory<TEntity>
    {
        ISpecification<TEntity> CreateSpecification();
        ISpecification<TEntity> CreateSpecification(IEntityFilter filter, bool forTotal);
    }
}
