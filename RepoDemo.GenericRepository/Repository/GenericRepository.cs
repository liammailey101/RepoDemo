using AutoMapper;
using AutoMapper.QueryableExtensions;
using RepoDemo.Common;
using RepoDemo.GenericRepository.Specification;
using Microsoft.EntityFrameworkCore;

namespace RepoDemo.GenericRepository.Repository
{
    /// <summary>
    /// A generic repository for performing CRUD operations on entities.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    public class GenericRepository<T>(DbContext context, ITelemetryClient telemetryClient, IMapper mapper) : IGenericRepository<T> where T : class
    {
        private readonly DbContext _context = context;
        private readonly DbSet<T> _dbSet = context.Set<T>();
        private readonly ITelemetryClient _telemetryClient = telemetryClient;
        private readonly IMapper _mapper = mapper;
        private readonly string _entityName = typeof(T).Name;
        private const int DefaultPageSize = 10;

        /// <summary>
        /// Gets an entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The entity if found; otherwise, null.</returns>
        /// <example>
        /// var customer = await repository.GetByIdAsync(1);
        /// </example>
        public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await ExecuteAsync(
               async () => await _dbSet.FindAsync(id, cancellationToken),
               $"An error occurred while getting the entity by ID.");
        }

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
        public async Task<TProjected?> GetByIdAsync<TProjected>(int id, CancellationToken cancellationToken = default)
        {
            return await ExecuteAsync(
                () => _dbSet.Where(e => EF.Property<int>(e, "Id") == id)
                            .ProjectTo<TProjected>(_mapper.ConfigurationProvider)
                            .FirstOrDefaultAsync(cancellationToken),
                $"An error occurred while getting the projected entity by ID.");
        }

        /// <summary>
        /// Gets a single entity that matches the specified criteria.
        /// </summary>
        /// <param name="spec">The specification containing the criteria.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The entity if found; otherwise, null.</returns>
        /// <example>
        /// var customer = await repository.GetSingleAsync(new CustomerSpecification());
        /// </example>
        public async Task<T?> GetSingleAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            return await ExecuteAsync(
                () => SpecificationEvaluator.ApplySpecification(_dbSet.AsQueryable(), spec).FirstOrDefaultAsync(cancellationToken),
                $"An error occurred while getting a single entity.");
        }

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
        public async Task<TProjected?> GetSingleAsync<TProjected>(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            return await ExecuteAsync(
                () => SpecificationEvaluator.ApplySpecification(_dbSet.AsQueryable(), spec)
                                            .ProjectTo<TProjected>(_mapper.ConfigurationProvider)
                                            .FirstOrDefaultAsync(cancellationToken),
                $"An error occurred while getting a single projected entity.");
        }

        /// <summary>
        /// Gets a list of entities that match the specified criteria.
        /// </summary>
        /// <param name="spec">The specification containing the criteria.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A list of entities.</returns>
        /// <example>
        /// var customers = await repository.GetListAsync(new CustomerSpecification());
        /// </example>
        public async Task<List<T>> GetListAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            return await ExecuteAsync(
                () => SpecificationEvaluator.ApplySpecification(_dbSet.AsQueryable(), spec).ToListAsync(cancellationToken),
                $"An error occurred while getting a list of entities.");
        }

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
        public async Task<List<TProjected>> GetListAsync<TProjected>(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            return await ExecuteAsync(
                () => SpecificationEvaluator.ApplySpecification(_dbSet.AsQueryable(), spec)
                                            .ProjectTo<TProjected>(_mapper.ConfigurationProvider)
                                            .ToListAsync(cancellationToken),
                $"An error occurred while getting a list of projected entities.");
        }

        /// <summary>
        /// Gets a paged result of entities that match the specified criteria.
        /// </summary>
        /// <param name="spec">The specification containing the criteria.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A paged result of entities.</returns>
        /// <example>
        /// var pagedCustomers = await repository.GetPagedAsync(new CustomerSpecification());
        /// </example>
        public async Task<PagedResult<T>> GetPagedAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            return await ExecuteAsync(
                async () =>
                {
                    var query = SpecificationEvaluator.ApplySpecification(_dbSet.AsQueryable(), spec);
                    var totalCount = await query.CountAsync(cancellationToken);
                    var result = new PagedResult<T>
                    {
                        TotalCount = totalCount,
                        PageSize = spec.Take ?? DefaultPageSize,
                        PageNumber = spec.Skip.HasValue && spec.Take.HasValue ? (spec.Skip.Value / spec.Take.Value) + 1 : 1,
                        Items = await query.ToListAsync(cancellationToken)
                    };
                    return result;
                },
                $"An error occurred while getting paged entities.");
        }

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
        public async Task<PagedResult<TProjected>> GetPagedAsync<TProjected>(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            return await ExecuteAsync(
                async () =>
                {
                    var query = SpecificationEvaluator.ApplySpecification(_dbSet.AsQueryable(), spec);
                    var totalCount = await query.CountAsync(cancellationToken);
                    var result = new PagedResult<TProjected>
                    {
                        TotalCount = totalCount,
                        PageSize = spec.Take ?? DefaultPageSize,
                        PageNumber = spec.Skip.HasValue && spec.Take.HasValue ? (spec.Skip.Value / spec.Take.Value) + 1 : 1,
                        Items = await query.ProjectTo<TProjected>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken)
                    };
                    return result;
                },
                $"An error occurred while getting paged projected entities.");
        }

        /// <summary>
        /// Counts the number of entities that match the specified criteria.
        /// </summary>
        /// <param name="spec">The specification containing the criteria.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>The number of entities that match the criteria.</returns>
        /// <example>
        /// var count = await repository.CountAsync(new CustomerSpecification());
        /// </example>
        public async Task<int> CountAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            return await ExecuteAsync(
                () => SpecificationEvaluator.ApplySpecification(_dbSet.AsQueryable(), spec).CountAsync(cancellationToken),
                $"An error occurred while counting entities.");
        }

        /// <summary>
        /// Adds a new entity to the database.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <example>
        /// await repository.AddAsync(new Customer { Name = "John Doe" });
        /// </example>
        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await ExecuteAsync(
                async () => await _dbSet.AddAsync(entity, cancellationToken),
                $"An error occurred while adding an entity.");
        }

        /// <summary>
        /// Adds a range of new entities to the database.
        /// </summary>
        /// <param name="entities">The entities to add.</param>
        /// <param name="asNoTracking">A flag to indicate if entities should be tracked.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <example>
        /// await repository.AddRangeAsync(new List<Customer> { new Customer { Name = "John Doe" }, new Customer { Name = "Jane Doe" } });
        /// </example>
        public async Task AddRangeAsync(IEnumerable<T> entities, bool asNoTracking = false, CancellationToken cancellationToken = default)
        {
            await ExecuteAsync(async () =>
            {
                var entityList = entities.ToList();
                if (asNoTracking)
                {
                    _context.ChangeTracker.AutoDetectChangesEnabled = false;
                    await _dbSet.AddRangeAsync(entityList, cancellationToken);
                    _context.ChangeTracker.AutoDetectChangesEnabled = true;
                    foreach (var entity in entityList)
                    {
                        _context.Entry(entity).State = EntityState.Detached;
                    }
                }
                else
                {
                    await _dbSet.AddRangeAsync(entityList, cancellationToken);
                }
            }, $"An error occurred while adding entities of type {_entityName}.");
        }

        /// <summary>
        /// Updates an existing entity in the database.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <example>
        /// repository.Update(existingCustomer);
        /// </example>
        public void Update(T entity)
        {
            Execute(
                () => _dbSet.Update(entity),
                $"An error occurred while updating an entity.");
        }

        /// <summary>
        /// Updates a range of existing entities in the database.
        /// </summary>
        /// <param name="entities">The entities to update.</param>
        /// <example>
        /// repository.UpdateRange(existingCustomers);
        /// </example>
        public void UpdateRange(IEnumerable<T> entities)
        {
            Execute(
                () => _dbSet.UpdateRange(entities),
                $"An error occurred while updating a range of entities.");
        }

        /// <summary>
        /// Deletes an entity from the database.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <example>
        /// repository.Delete(existingCustomer);
        /// </example>
        public void Delete(T entity)
        {
            Execute(
                () => _dbSet.Remove(entity),
                $"An error occurred while deleting an entity.");
        }

        /// <summary>
        /// Deletes a range of entities from the database.
        /// </summary>
        /// <param name="entities">The entities to delete.</param>
        /// <example>
        /// repository.DeleteRange(existingCustomers);
        /// </example>
        public void DeleteRange(IEnumerable<T> entities)
        {
            Execute(
                () => _dbSet.RemoveRange(entities),
                $"An error occurred while deleting a range of entities.");
        }

        /// <summary>
        /// Deletes an entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the entity to delete.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <example>
        /// await repository.DeleteByIdAsync(1);
        /// </example>
        public async Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            await ExecuteAsync(
                async () =>
                {
                    var entity = await _dbSet.FindAsync(id, cancellationToken);
                    if (entity != null)
                    {
                        _dbSet.Remove(entity);
                    }
                },
                $"An error occurred while deleting an entity by ID.");
        }

        /// <summary>
        /// Executes an asynchronous action with error handling.
        /// </summary>
        /// <param name="action">The asynchronous action to execute.</param>
        /// <param name="errorMessage">The error message to include in the exception if an error occurs.</param>
        private async Task ExecuteAsync(Func<Task> action, string errorMessage)
        {
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex);
                throw new RepositoryException($"{_entityName}Repository: {errorMessage}", ex);
            }
        }

        /// <summary>
        /// Executes an asynchronous function with error handling and returns the result.
        /// </summary>
        /// <typeparam name="TE">The type of the result.</typeparam>
        /// <param name="action">The asynchronous function to execute.</param>
        /// <param name="errorMessage">The error message to include in the exception if an error occurs.</param>
        /// <returns>The result of the asynchronous function.</returns>
        private async Task<TE> ExecuteAsync<TE>(Func<Task<TE>> action, string errorMessage)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex);
                throw new RepositoryException($"{_entityName}Repository: {errorMessage}", ex);
            }
        }

        /// <summary>
        /// Executes a synchronous action with error handling.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="errorMessage">The error message to include in the exception if an error occurs.</param>
        private void Execute(Action action, string errorMessage)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                _telemetryClient.TrackException(ex);
                throw new RepositoryException($"{_entityName}Repository: {errorMessage}", ex);
            }
        }
    }
}