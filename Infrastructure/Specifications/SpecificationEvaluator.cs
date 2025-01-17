﻿using Domain.Base;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specifications
{
    public static class SpecificationEvaluator<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification)
        {
            var query = inputQuery;

            if (specification != null)
            {
                // modify the IQueryable using the specification's criteria expression
                if (specification.Criteria != null)
                {
                    query = query.Where(specification.Criteria);
                }

                // Includes all expression-based includes
                query = specification.Includes.Aggregate(query,
                                        (current, include) => current.Include(include).AsSplitQuery());

                // Include any string-based include statements
                query = specification.IncludeStrings.Aggregate(query,
                                        (current, include) => current.Include(include).AsSplitQuery());

                // Apply ordering if expressions are set
                if (specification.OrderBy != null)
                {
                    query = query.OrderBy(specification.OrderBy);
                }
                else if (specification.OrderByDescending != null)
                {
                    query = query.OrderByDescending(specification.OrderByDescending);
                }

                // Apply paging if enabled
                if (specification.IsPagingEnabled)
                {
                    query = query.Skip(specification.Skip)
                                 .Take(specification.Take);
                }
            }

            return query;
        }
    }
}
