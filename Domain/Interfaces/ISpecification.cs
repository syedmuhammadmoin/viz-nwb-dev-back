using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ISpecification<T>
    {
        // Filter Conditions
        Expression<Func<T, bool>> Criteria { get; }
        // Include collection to load related data
        List<Expression<Func<T, object>>> Includes { get; }
        // Order By Ascending
        Expression<Func<T, object>> OrderBy { get; }
        // Order By Descending
        Expression<Func<T, object>> OrderByDescending { get; }

        //For Pagination
        int Take { get; }
        int Skip { get; }
        bool IsPagingEnabled { get; }
    }
}
