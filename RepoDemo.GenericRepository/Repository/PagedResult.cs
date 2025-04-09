namespace RepoDemo.GenericRepository.Repository
{
    /// <summary>
    /// Represents a paged result of a query, including the items and pagination details.
    /// </summary>
    /// <typeparam name="T">The type of the items in the paged result.</typeparam>
    public class PagedResult<T>
    {
        /// <summary>
        /// Gets or sets the list of items in the current page.
        /// </summary>
        /// <example>
        /// var pagedResult = new PagedResult<Customer> { Items = new List<Customer> { new Customer { Name = "John Doe" } } };
        /// </example>
        public List<T> Items { get; set; }

        /// <summary>
        /// Gets or sets the total count of items across all pages.
        /// </summary>
        /// <example>
        /// var pagedResult = new PagedResult<Customer> { TotalCount = 100 };
        /// </example>
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or sets the current page number.
        /// </summary>
        /// <example>
        /// var pagedResult = new PagedResult<Customer> { PageNumber = 1 };
        /// </example>
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the size of the page (number of items per page).
        /// </summary>
        /// <example>
        /// var pagedResult = new PagedResult<Customer> { PageSize = 10 };
        /// </example>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets the total number of pages.
        /// </summary>
        /// <example>
        /// var totalPages = pagedResult.TotalPages;
        /// </example>
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

        /// <summary>
        /// Initializes a new instance of the <see cref="PagedResult{T}"/> class.
        /// </summary>
        /// <example>
        /// var pagedResult = new PagedResult<Customer>();
        /// </example>
        public PagedResult()
        {
            Items = new List<T>();
        }
    }
}