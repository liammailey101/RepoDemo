namespace RepoDemo.GenericRepository.Repository
{
    /// <summary>
    /// Represents errors that occur during repository operations.
    /// </summary>
    public class RepositoryException(string message, Exception innerException) : Exception(message, innerException)
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        /// <example>
        /// throw new RepositoryException("An error occurred while accessing the repository.", ex);
        /// </example>
    }
}