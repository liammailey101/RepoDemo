using System.Linq.Expressions;
using MetaComplianceRepoDemo.Data.Specification;
using Microsoft.EntityFrameworkCore;

namespace MetaComplianceRepoDemo.Data.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id) ?? throw new KeyNotFoundException();
        }

        public async Task<T> GetSingleAsync(ISpecification<T> spec)
        {
            var query = ApplySpecification(spec);
            return await query.FirstOrDefaultAsync() ?? throw new KeyNotFoundException();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<List<T>> GetListAsync(ISpecification<T> spec)
        {
            var query = ApplySpecification(spec);
            return await query.ToListAsync();
        }

        public async Task<PagedResult<T>> GetPagedAsync(ISpecification<T> spec)
        {
            var query = ApplySpecification(spec);
            var totalCount = await query.CountAsync();

            var result = new PagedResult<T>
            {
                TotalCount = totalCount,
                PageSize = spec.Take ?? 10,
                PageNumber = spec.Skip.HasValue && spec.Take.HasValue ? (spec.Skip.Value / spec.Take.Value) + 1 : 1,
                Items = await query.ToListAsync()
            };

            return result;
        }

        public async Task<TProjected> GetSingleProjectedAsync<TProjected>(
            ISpecification<T> spec,
            Expression<Func<T, TProjected>> projection)
        {
            var query = ApplySpecification(spec);
            return await query.Select(projection).FirstOrDefaultAsync()
                ?? throw new KeyNotFoundException();
        }

        public async Task<List<TProjected>> GetListProjectedAsync<TProjected>(
            ISpecification<T> spec,
            Expression<Func<T, TProjected>> projection)
        {
            var query = ApplySpecification(spec);
            return await query.Select(projection).ToListAsync();
        }

        public async Task<PagedResult<TProjected>> GetPagedProjectedAsync<TProjected>(
            ISpecification<T> spec,
            Expression<Func<T, TProjected>> projection)
        {
            var query = ApplySpecification(spec);
            var totalCount = await query.CountAsync();

            var result = new PagedResult<TProjected>
            {
                TotalCount = totalCount,
                PageSize = spec.Take ?? 10,
                PageNumber = spec.Skip.HasValue && spec.Take.HasValue ? (spec.Skip.Value / spec.Take.Value) + 1 : 1,
                Items = await query.Select(projection).ToListAsync()
            };

            return result;
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            var query = ApplySpecification(spec);
            return await query.CountAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            var query = spec.AsNoTracking
                ? _dbSet.AsNoTracking()
                : _dbSet.AsTracking();

            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            query = spec.Includes.Aggregate(query,
                (current, include) => current.Include(include));

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
