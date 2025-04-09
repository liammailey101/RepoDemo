using System.Linq.Expressions;

namespace RepoDemo.GenericRepository.Specification
{
    /// <summary>
    /// Defines a specification for querying entities, including criteria, includes, ordering, and pagination.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    public interface ISpecification<T>
    {
        /// <summary>
        /// Gets the criteria expression for filtering entities.
        /// </summary>
        /// <example>
        /// var spec = new CustomerSpecification { Criteria = c => c.Name == "John Doe" };
        /// </example>
        Expression<Func<T, bool>>? Criteria { get; }

        /// <summary>
        /// Gets the list of include expressions for including related entities.
        /// </summary>
        /// <example>
        /// var spec = new CustomerSpecification();
        /// spec.AddInclude(c => c.Orders);
        /// </example>
        List<Expression<Func<T, object>>> Includes { get; }

        /// <summary>
        /// Gets the expression for ordering entities in ascending order.
        /// </summary>
        /// <example>
        /// var spec = new CustomerSpecification();
        /// spec.ApplyOrderBy(c => c.Name);
        /// </example>
        Expression<Func<T, object>>? OrderBy { get; }

        /// <summary>
        /// Gets the expression for ordering entities in descending order.
        /// </summary>
        /// <example>
        /// var spec = new CustomerSpecification();
        /// spec.ApplyOrderByDescending(c => c.Name);
        /// </example>
        Expression<Func<T, object>>? OrderByDescending { get; }

        /// <summary>
        /// Gets the number of entities to take.
        /// </summary>
        /// <example>
        /// var spec = new CustomerSpecification();
        /// spec.ApplyPaging(0, 10);
        /// </example>
        int? Take { get; }

        /// <summary>
        /// Gets the number of entities to skip.
        /// </summary>
        /// <example>
        /// var spec = new CustomerSpecification();
        /// spec.ApplyPaging(10, 10);
        /// </example>
        int? Skip { get; }

        /// <summary>
        /// Gets a value indicating whether paging is enabled.
        /// </summary>
        /// <example>
        /// var isPagingEnabled = spec.IsPagingEnabled;
        /// </example>
        bool IsPagingEnabled { get; }

        /// <summary>
        /// Gets a value indicating whether the query should be executed with no tracking.
        /// </summary>
        /// <example>
        /// var spec = new CustomerSpecification();
        /// spec.SetAsNoTracking();
        /// </example>
        bool AsNoTracking { get; }
    }
}