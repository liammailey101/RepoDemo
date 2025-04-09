using System.Linq.Expressions;
using MetaComplianceRepoDemo.Data.Specification;

namespace MetaComplianceRepoDemo.Data.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<T> GetSingleAsync(ISpecification<T> spec);
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetListAsync(ISpecification<T> spec);
        Task<PagedResult<T>> GetPagedAsync(ISpecification<T> spec);
        Task<TProjected> GetSingleProjectedAsync<TProjected>(ISpecification<T> spec, Expression<Func<T, TProjected>> projection);
        Task<List<TProjected>> GetListProjectedAsync<TProjected>(ISpecification<T> spec, Expression<Func<T, TProjected>> projection);
        Task<PagedResult<TProjected>> GetPagedProjectedAsync<TProjected>(ISpecification<T> spec, Expression<Func<T, TProjected>> projection);
        Task<int> CountAsync(ISpecification<T> spec);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
    }
}
