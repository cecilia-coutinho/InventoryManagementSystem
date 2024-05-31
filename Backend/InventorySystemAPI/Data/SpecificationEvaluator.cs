using InventorySystemAPI.Specifications;
using Microsoft.EntityFrameworkCore;

namespace InventorySystemAPI.Data
{
    public class SpecificationEvaluator<T> where T : class
    {
        public static IQueryable<T> GetQuery(
            IQueryable<T> inputQuery,
            ISpecification<T> specification)
        {
            ArgumentNullException.ThrowIfNull(inputQuery);
            ArgumentNullException.ThrowIfNull(specification);

            var query = inputQuery;

            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }

            if (specification.Includes != null)
            {
                query = specification.Includes.Aggregate(query,
                                       (current, include) => current.Include(include));
            }

            if (specification.OrderBy != null)
            {
                query = query.OrderBy(specification.OrderBy);
            }
            else if (specification.OrderByDescending != null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }
            if (specification.GroupBy != null)
            {
                query = query.GroupBy(specification.GroupBy).SelectMany(x => x);
            }
            if (specification.IsPagingEnabled)
            {
                if (specification.Skip.HasValue && specification.Take.HasValue)
                {
                    query = query.Skip(specification.Skip.Value)
                                 .Take(specification.Take.Value);
                }
            }

            return query;
        }
    }
}
