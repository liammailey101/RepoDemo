using FluentAssertions;
using RepoDemo.GenericRepository.Repository;

namespace RepoDemo.GenericRepository.Test
{
    [TestFixture]
    public class RepositoryExceptionTests
    {
        [Test]
        public void Constructor_ShouldInitializeWithMessageAndInnerException()
        {
            // Arrange
            var message = "Test exception message";
            var innerException = new Exception("Inner exception message");

            // Act
            var exception = new RepositoryException(message, innerException);

            // Assert
            exception.Message.Should().Be(message);
            exception.InnerException.Should().Be(innerException);
        }

        [Test]
        public void Constructor_ShouldInitializeWithMessageAndNullInnerException()
        {
            // Arrange
            var message = "Test exception message";

            // Act
            #pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var exception = new RepositoryException(message, null);
            #pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            // Assert
            exception.Message.Should().Be(message);
            exception.InnerException.Should().BeNull();
        }
    }
}
