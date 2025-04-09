using Microsoft.EntityFrameworkCore;

namespace RepoDemo.GenericRepository.Specification
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<T> ApplySpecification<T>(IQueryable<T> inputQuery, ISpecification<T> spec) where T : class
        {
            var query = spec.AsNoTracking
                ? inputQuery.AsNoTracking()
                : inputQuery.AsTracking();

            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            if (spec.Includes != null)
            {
                query = spec.Includes.Aggregate(query,
                    (current, include) => current.Include(include));
            }

            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            else if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            if (spec.IsPagingEnabled)
            {
                query = query.Skip(spec.Skip ?? 0)
                            .Take(spec.Take ?? 10);
            }

            return query;
        }
    }
}