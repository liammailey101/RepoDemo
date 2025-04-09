using RepoDemo.GenericRepository.Specification;
using System.Threading;

namespace RepoDemo.GenericRepository.Repository
{
    /// <summary>
    /// A generic repository interface for performing CRUD operations on entities.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Gets an entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The entity if found; otherwise, null.</returns>
        /// <example>
        /// var customer = await repository.GetByIdAsync(1);
        /// </example>
        Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a projected entity by its ID.
        /// </summary>
        /// <typeparam name="TProjected">The type of the projected entity.</typeparam>
        /// <param name="id">The ID of the entity.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The projected entity if found; otherwise, null.</returns>
        /// <example>
        /// var customerDto = await repository.GetByIdAsync<CustomerDto>(1);
        /// </example>
        Task<TProjected?> GetByIdAsync<TProjected>(int id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a single entity that matches the specified criteria.
        /// </summary>
        /// <param name="spec">The specification containing the criteria.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The entity if found; otherwise, null.</returns>
        /// <example>
        /// var customer = await repository.GetSingleAsync(new CustomerSpecification());
        /// </example>
        Task<T?> GetSingleAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a single projected entity that matches the specified criteria.
        /// </summary>
        /// <typeparam name="TProjected">The type of the projected entity.</typeparam>
        /// <param name="spec">The specification containing the criteria.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The projected entity if found; otherwise, null.</returns>
        /// <example>
        /// var customerDto = await repository.GetSingleAsync<CustomerDto>(new CustomerSpecification());
        /// </example>
        Task<TProjected?> GetSingleAsync<TProjected>(ISpecification<T> spec, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a list of entities that match the specified criteria.
        /// </summary>
        /// <param name="spec">The specification containing the criteria.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A list of entities.</returns>
        /// <example>
        /// var customers = await repository.GetListAsync(new CustomerSpecification());
        /// </example>
        Task<List<T>> GetListAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a list of projected entities that match the specified criteria.
        /// </summary>
        /// <typeparam name="TProjected">The type of the projected entities.</typeparam>
        /// <param name="spec">The specification containing the criteria.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A list of projected entities.</returns>
        /// <example>
        /// var customerDtos = await repository.GetListAsync<CustomerDto>(new CustomerSpecification());
        /// </example>
        Task<List<TProjected>> GetListAsync<TProjected>(ISpecification<T> spec, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a paged result of entities that match the specified criteria.
        /// </summary>
        /// <param name="spec">The specification containing the criteria.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A paged result of entities.</returns>
        /// <example>
        /// var pagedCustomers = await repository.GetPagedAsync(new CustomerSpecification());
        /// </example>
        Task<PagedResult<T>> GetPagedAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets a paged result of projected entities that match the specified criteria.
        /// </summary>
        /// <typeparam name="TProjected">The type of the projected entities.</typeparam>
        /// <param name="spec">The specification containing the criteria.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A paged result of projected entities.</returns>
        /// <example>
        /// var pagedCustomerDtos = await repository.GetPagedAsync<CustomerDto>(new CustomerSpecification());
        /// </example>
        Task<PagedResult<TProjected>> GetPagedAsync<TProjected>(ISpecification<T> spec, CancellationToken cancellationToken = default);

        /// <summary>
        /// Counts the number of entities that match the specified criteria.
        /// </summary>
        /// <param name="spec">The specification containing the criteria.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The number of entities that match the criteria.</returns>
        /// <example>
        /// var count = await repository.CountAsync(new CustomerSpecification());
        /// </example>
        Task<int> CountAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a new entity to the database.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <example>
        /// await repository.AddAsync(new Customer { Name = "John Doe" });
        /// </example>
        Task AddAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds a range of new entities to the database.
        /// </summary>
        /// <param name="entities">The entities to add.</param>
        /// <param name="asNoTracking">A flag to indicate if entities should be tracked.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <example>
        /// await repository.AddRangeAsync(new List<Customer> { new Customer { Name = "John Doe" }, new Customer { Name = "Jane Doe" } });
        /// </example>
        Task AddRangeAsync(IEnumerable<T> entities, bool asNoTracking = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an existing entity in the database.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <example>
        /// repository.Update(existingCustomer);
        /// </example>
        void Update(T entity);

        /// <summary>
        /// Updates a range of existing entities in the database.
        /// </summary>
        /// <param name="entities">The entities to update.</param>
        /// <example>
        /// repository.UpdateRange(existingCustomers);
        /// </example>
        void UpdateRange(IEnumerable<T> entities);

        /// <summary>
        /// Deletes an entity from the database.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <example>
        /// repository.Delete(existingCustomer);
        /// </example>
        void Delete(T entity);

        /// <summary>
        /// Deletes a range of entities from the database.
        /// </summary>
        /// <param name="entities">The entities to delete.</param>
        /// <example>
        /// repository.DeleteRange(existingCustomers);
        /// </example>
        void DeleteRange(IEnumerable<T> entities);

        /// <summary>
        /// Deletes an entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the entity to delete.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <example>
        /// await repository.DeleteByIdAsync(1);
        /// </example>
        Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default);
    }
}