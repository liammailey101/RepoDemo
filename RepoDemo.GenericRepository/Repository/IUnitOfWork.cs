using Microsoft.EntityFrameworkCore;

namespace RepoDemo.GenericRepository.Repository
{
    /// <summary>
    /// Represents a unit of work that encapsulates a set of operations to be performed as a single transaction.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Gets the repository for the specified entity type.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <returns>The repository for the specified entity type.</returns>
        /// <example>
        /// var customerRepository = unitOfWork.Repository<Customer>();
        /// </example>
        IGenericRepository<T> Repository<T>() where T : class;

        /// <summary>
        /// Saves all changes made in this unit of work to the database.
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        /// <example>
        /// var changes = await unitOfWork.CompleteAsync();
        /// </example>
        Task<int> CompleteAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes a set of operations within a transaction.
        /// If any operation fails, the transaction is rolled back.
        /// </summary>
        /// <param name="action">The asynchronous action to execute within the transaction.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <example>
        /// await unitOfWork.ExecuteInTransactionAsync(async () =>
        /// {
        ///     // Perform multiple operations
        ///     repository.Add(entity);
        ///     repository.Update(anotherEntity);
        ///     // If any operation fails, the transaction is rolled back
        /// });
        /// </example>
        Task ExecuteInTransactionAsync(Func<Task> action, CancellationToken cancellationToken = default);
    }
}