using Application.Contracts.Filters;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications;

internal class SpecificationFactory<TEntity> : ISpecificationFactory<TEntity>
{
    ISpecification<TEntity> ISpecificationFactory<TEntity>.CreateSpecification(IEntityFilter filter, bool forTotal)
    {


        switch (typeof(TEntity))
        {
            //case Type t when t == typeof(Currency):
            //    return (ISpecification<TEntity>)new CurrencySpec(filter, false);

            default:
                throw new InvalidOperationException($"Unsupported TEntity type: {typeof(TEntity)}");
        }
    }

    ISpecification<TEntity> ISpecificationFactory<TEntity>.CreateSpecification()
    {
        switch (typeof(TEntity))
        {
            case Type t when t == typeof(Currency):
                return (ISpecification<TEntity>)new CurrencySpec();

            default:
                throw new InvalidOperationException($"Unsupported TEntity type: {typeof(TEntity)}");
        }

    }

}

