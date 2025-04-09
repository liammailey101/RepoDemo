using System.Linq.Expressions;
using FluentAssertions;
using RepoDemo.GenericRepository.Specification;

namespace RepoDemo.GenericRepository.Test
{
    public class BaseSpecificationTests
    {
        [Test]
        public void Criteria_ShouldBeSetCorrectly()
        {
            // Arrange
            Expression<Func<TestEntity, bool>> criteria = e => e.Name == "John Doe";

            // Act
            var spec = new TestSpecification(criteria);

            // Assert
            spec.Criteria.Should().Be(criteria);
        }

        [Test]
        public void AddInclude_ShouldAddIncludeExpression()
        {
            // Arrange
            var spec = new TestSpecification(e => e.Name == "John Doe");
            Expression<Func<TestEntity, object>> includeExpression = e => e.Name;

            // Act
            spec.AddIncludeTest(includeExpression);

            // Assert
            spec.Includes.Should().ContainSingle().Which.Should().Be(includeExpression);
        }

        [Test]
        public void ApplyPaging_ShouldSetSkipAndTake()
        {
            // Arrange
            var spec = new TestSpecification(e => e.Name == "John Doe");

            // Act
            spec.ApplyPagingTest(10, 20);

            // Assert
            spec.Skip.Should().Be(10);
            spec.Take.Should().Be(20);
            spec.IsPagingEnabled.Should().BeTrue();
        }

        [Test]
        public void ApplyOrderBy_ShouldSetOrderByExpression()
        {
            // Arrange
            var spec = new TestSpecification(e => e.Name == "John Doe");
            Expression<Func<TestEntity, object>> orderByExpression = e => e.Name;

            // Act
            spec.ApplyOrderByTest(orderByExpression);

            // Assert
            spec.OrderBy.Should().Be(orderByExpression);
        }

        [Test]
        public void ApplyOrderByDescending_ShouldSetOrderByDescendingExpression()
        {
            // Arrange
            var spec = new TestSpecification(e => e.Name == "John Doe");
            Expression<Func<TestEntity, object>> orderByDescendingExpression = e => e.Name;

            // Act
            spec.ApplyOrderByDescendingTest(orderByDescendingExpression);

            // Assert
            spec.OrderByDescending.Should().Be(orderByDescendingExpression);
        }

        [Test]
        public void SetAsNoTracking_ShouldSetAsNoTracking()
        {
            // Arrange
            var spec = new TestSpecification(e => e.Name == "John Doe");

            // Act
            spec.SetAsNoTrackingTest(true);

            // Assert
            spec.AsNoTracking.Should().BeTrue();
        }

        [Test]
        public void SetAsNoTracking_ShouldUnsetAsNoTracking()
        {
            // Arrange
            var spec = new TestSpecification(e => e.Name == "John Doe");

            // Act
            spec.SetAsNoTrackingTest(false);

            // Assert
            spec.AsNoTracking.Should().BeFalse();
        }

        private class TestEntity
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }

        private class TestSpecification : BaseSpecification<TestEntity>
        {
            public TestSpecification(Expression<Func<TestEntity, bool>> criteria)
            {
                Criteria = criteria;
            }

            public void AddIncludeTest(Expression<Func<TestEntity, object>> includeExpression)
            {
                AddInclude(includeExpression);
            }

            public void ApplyPagingTest(int skip, int take)
            {
                ApplyPaging(skip, take);
            }

            public void ApplyOrderByTest(Expression<Func<TestEntity, object>> orderByExpression)
            {
                ApplyOrderBy(orderByExpression);
            }

            public void ApplyOrderByDescendingTest(Expression<Func<TestEntity, object>> orderByDescendingExpression)
            {
                ApplyOrderByDescending(orderByDescendingExpression);
            }

            public void SetAsNoTrackingTest(bool asNoTracking)
            {
                SetAsNoTracking(asNoTracking);
            }
        }
    }
}