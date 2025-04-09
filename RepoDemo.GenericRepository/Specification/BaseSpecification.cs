using System.Linq.Expressions;

namespace RepoDemo.GenericRepository.Specification
{
    /// <summary>
    /// Base class for specifications, providing a way to define query criteria, includes, ordering, and pagination.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    public abstract class BaseSpecification<T> : ISpecification<T>
    {
        /// <summary>
        /// Gets the criteria expression for filtering entities.
        /// </summary>
        /// <example>
        /// var spec = new CustomerSpecification { Criteria = c => c.Name == "John Doe" };
        /// </example>
        public Expression<Func<T, bool>>? Criteria { get; protected set; }

        /// <summary>
        /// Gets the list of include expressions for including related entities.
        /// </summary>
        /// <example>
        /// var spec = new CustomerSpecification();
        /// spec.AddInclude(c => c.Orders);
        /// </example>
        public List<Expression<Func<T, object>>> Includes { get; } = new();

        /// <summary>
        /// Gets the expression for ordering entities in ascending order.
        /// </summary>
        /// <example>
        /// var spec = new CustomerSpecification();
        /// spec.ApplyOrderBy(c => c.Name);
        /// </example>
        public Expression<Func<T, object>>? OrderBy { get; protected set; }

        /// <summary>
        /// Gets the expression for ordering entities in descending order.
        /// </summary>
        /// <example>
        /// var spec = new CustomerSpecification();
        /// spec.ApplyOrderByDescending(c => c.Name);
        /// </example>
        public Expression<Func<T, object>>? OrderByDescending { get; protected set; }

        /// <summary>
        /// Gets the number of entities to take.
        /// </summary>
        /// <example>
        /// var spec = new CustomerSpecification();
        /// spec.ApplyPaging(0, 10);
        /// </example>
        public int? Take { get; protected set; }

        /// <summary>
        /// Gets the number of entities to skip.
        /// </summary>
        /// <example>
        /// var spec = new CustomerSpecification();
        /// spec.ApplyPaging(10, 10);
        /// </example>
        public int? Skip { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether paging is enabled.
        /// </summary>
        /// <example>
        /// var isPagingEnabled = spec.IsPagingEnabled;
        /// </example>
        public bool IsPagingEnabled => Take.HasValue || Skip.HasValue;

        /// <summary>
        /// Gets a value indicating whether the query should be executed with no tracking.
        /// </summary>
        /// <example>
        /// var spec = new CustomerSpecification();
        /// spec.SetAsNoTracking();
        /// </example>
        public bool AsNoTracking { get; protected set; }

        /// <summary>
        /// Adds an include expression to the specification.
        /// </summary>
        /// <param name="includeExpression">The include expression.</param>
        /// <example>
        /// spec.AddInclude(c => c.Orders);
        /// </example>
        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        /// <summary>
        /// Applies paging to the specification.
        /// </summary>
        /// <param name="skip">The number of entities to skip.</param>
        /// <param name="take">The number of entities to take.</param>
        /// <example>
        /// spec.ApplyPaging(0, 10);
        /// </example>
        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
        }

        /// <summary>
        /// Applies ordering in ascending order to the specification.
        /// </summary>
        /// <param name="orderByExpression">The order by expression.</param>
        /// <example>
        /// spec.ApplyOrderBy(c => c.Name);
        /// </example>
        protected void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        /// <summary>
        /// Applies ordering in descending order to the specification.
        /// </summary>
        /// <param name="orderByDescendingExpression">The order by descending expression.</param>
        /// <example>
        /// spec.ApplyOrderByDescending(c => c.Name);
        /// </example>
        protected void ApplyOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
        }

        /// <summary>
        /// Sets the query to be executed with no tracking.
        /// </summary>
        /// <param name="asNoTracking">Whether to execute the query with no tracking.</param>
        /// <example>
        /// spec.SetAsNoTracking();
        /// </example>
        protected void SetAsNoTracking(bool asNoTracking = true)
        {
            AsNoTracking = asNoTracking;
        }
    }
}