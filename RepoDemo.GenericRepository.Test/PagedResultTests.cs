using FluentAssertions;
using RepoDemo.GenericRepository.Repository;

namespace RepoDemo.GenericRepository.Test;

[TestFixture]
public class PagedResultTests
{
    [Test]
    public void Constructor_ShouldInitializeWithEmptyItems()
    {
        // Act
        var pagedResult = new PagedResult<string>();

        // Assert
        pagedResult.Items.Should().NotBeNull();
        pagedResult.Items.Should().BeEmpty();
    }

    [Test]
    [TestCase(25, 10, 3)]
    [TestCase(100, 10, 10)]
    [TestCase(0, 10, 0)]
    [TestCase(25, 1, 25)]
    [TestCase(25, 25, 1)]
    [TestCase(26, 25, 2)]
    public void TotalPages_ShouldReturnCorrectValue(int totalCount, int pageSize, int expectedTotalPages)
    {
        // Arrange
        var pagedResult = new PagedResult<string>
        {
            TotalCount = totalCount,
            PageSize = pageSize
        };

        // Act
        var totalPages = pagedResult.TotalPages;

        // Assert
        totalPages.Should().Be(expectedTotalPages);
    }

    [Test]
    public void Items_ShouldGetAndSetCorrectly()
    {
        // Arrange
        var items = new List<string> { "Item1", "Item2" };
        var pagedResult = new PagedResult<string>
        {
            Items = items
        };

        // Act
        var resultItems = pagedResult.Items;

        // Assert
        resultItems.Should().BeSameAs(items);
    }

    [Test]
    public void TotalCount_ShouldGetAndSetCorrectly()
    {
        // Arrange
        var pagedResult = new PagedResult<string>
        {
            TotalCount = 100
        };

        // Act
        var totalCount = pagedResult.TotalCount;

        // Assert
        totalCount.Should().Be(100);
    }

    [Test]
    public void PageNumber_ShouldGetAndSetCorrectly()
    {
        // Arrange
        var pagedResult = new PagedResult<string>
        {
            PageNumber = 2
        };

        // Act
        var pageNumber = pagedResult.PageNumber;

        // Assert
        pageNumber.Should().Be(2);
    }

    [Test]
    public void PageSize_ShouldGetAndSetCorrectly()
    {
        // Arrange
        var pagedResult = new PagedResult<string>
        {
            PageSize = 20
        };

        // Act
        var pageSize = pagedResult.PageSize;

        // Assert
        pageSize.Should().Be(20);
    }
}
