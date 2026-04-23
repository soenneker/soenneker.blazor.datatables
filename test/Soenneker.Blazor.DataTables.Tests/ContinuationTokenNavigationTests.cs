using Microsoft.Extensions.Logging;
using Moq;
using AwesomeAssertions;
using Soenneker.Blazor.DataTables.Options;

namespace Soenneker.Blazor.DataTables.Tests;

public sealed class ContinuationTokenNavigationTests
{
    private readonly Mock<ILogger<DataTable>> _mockLogger = new();

    [Test]
    public void ShouldSupportFullNavigationFlow()
    {
        // Arrange
        var paging = new DataTableContinuationTokenPaging();
        var pageSize = 10;

        // Simulate navigating through pages and storing tokens
        // Page 0: Start from beginning
        paging.SetContinuationToken(0, null); // First page has no token
        paging.UpdateFromResponse(pageSize, 10, "token_page_1");
        
        // Page 1: Navigate to next page
        paging.UpdateVirtualPage(10, pageSize, "token_page_1"); // Set current page to 1
        paging.UpdateFromResponse(pageSize, 10, "token_page_2");
        
        // Page 2: Navigate to next page
        paging.UpdateVirtualPage(20, pageSize, "token_page_2"); // Set current page to 2
        paging.UpdateFromResponse(pageSize, 10, "token_page_3");
        
        // Page 3: Navigate to next page
        paging.UpdateVirtualPage(30, pageSize, "token_page_3"); // Set current page to 3
        paging.UpdateFromResponse(pageSize, 10, null); // Last page

        // Act & Assert - Test backward navigation
        string? tokenForPage2 = paging.GetBestTokenForPage(2, pageSize);
        tokenForPage2.Should().Be("token_page_2");

        string? tokenForPage1 = paging.GetBestTokenForPage(1, pageSize);
        tokenForPage1.Should().Be("token_page_1");

        string? tokenForPage0 = paging.GetBestTokenForPage(0, pageSize);
        tokenForPage0.Should().BeNull(); // First page has no token

        // Test forward navigation
        string? tokenForPage3 = paging.GetBestTokenForPage(3, pageSize);
        tokenForPage3.Should().Be("token_page_3");
    }

    [Test]
    public void ShouldReturnSameTokenWhenNavigatingBackToPage()
    {
        // Arrange
        var paging = new DataTableContinuationTokenPaging();
        var pageSize = 10;

        // Simulate navigating through pages and storing original tokens
        // Page 0: Start from beginning
        paging.SetContinuationToken(0, null); // First page has no token
        paging.UpdateFromResponse(pageSize, 10, "token_page_1");
        
        // Page 1: Navigate to next page
        paging.UpdateVirtualPage(10, pageSize, "token_page_1"); // Set current page to 1
        paging.UpdateFromResponse(pageSize, 10, "token_page_2");
        
        // Page 2: Navigate to next page
        paging.UpdateVirtualPage(20, pageSize, "token_page_2"); // Set current page to 2
        paging.UpdateFromResponse(pageSize, 10, "token_page_3");
        
        // Page 3: Navigate to next page
        paging.UpdateVirtualPage(30, pageSize, "token_page_3"); // Set current page to 3
        paging.UpdateFromResponse(pageSize, 10, null); // Last page

        // Act - Navigate back to page 2 multiple times
        string? tokenForPage2First = paging.GetBestTokenForPage(2, pageSize);
        string? tokenForPage2Second = paging.GetBestTokenForPage(2, pageSize);
        string? tokenForPage2Third = paging.GetBestTokenForPage(2, pageSize);

        // Assert - Should always return the same token
        tokenForPage2First.Should().Be("token_page_2");
        tokenForPage2Second.Should().Be("token_page_2");
        tokenForPage2Third.Should().Be("token_page_2");
        tokenForPage2First.Should().Be(tokenForPage2Second);
        tokenForPage2Second.Should().Be(tokenForPage2Third);
    }

    [Test]
    public void ShouldHandleNavigationWithMissingTokens()
    {
        // Arrange
        var paging = new DataTableContinuationTokenPaging();
        var pageSize = 10;

        // Store tokens for pages 0, 2, 4 (missing 1 and 3)
        paging.SetContinuationToken(0, null);
        paging.SetContinuationToken(2, "token_page_2");
        paging.SetContinuationToken(4, "token_page_4");

        // Act & Assert - Test navigation to missing pages
        string? tokenForPage1 = paging.GetBestTokenForPage(1, pageSize);
        tokenForPage1.Should().BeNull(); // Should use page 0 token (null)

        string? tokenForPage3 = paging.GetBestTokenForPage(3, pageSize);
        tokenForPage3.Should().Be("token_page_2"); // Should use page 2 token

        string? tokenForPage5 = paging.GetBestTokenForPage(5, pageSize);
        tokenForPage5.Should().Be("token_page_4"); // Should use page 4 token
    }

    [Test]
    public void ShouldHandleResetCorrectly()
    {
        // Arrange
        var paging = new DataTableContinuationTokenPaging();
        var pageSize = 10;

        // Store some tokens
        paging.SetContinuationToken(1, "token_page_1");
        paging.SetContinuationToken(2, "token_page_2");
        paging.UpdateFromResponse(pageSize, 10, "token_page_3");

        // Act
        paging.Reset();

        // Assert
        paging.GetContinuationToken(1).Should().BeNull();
        paging.GetContinuationToken(2).Should().BeNull();
        paging.GetPageRecordCount(0).Should().Be(0);
        paging.EstimatedTotalRecords.Should().Be(0);
        paging.HasMorePages.Should().BeTrue();
    }

    [Test]
    public void ShouldCalculateVirtualStartCorrectly()
    {
        // Arrange
        var paging = new DataTableContinuationTokenPaging();
        var pageSize = 10;

        // Act & Assert - Test with default current page (0)
        paging.CalculateVirtualStart(pageSize).Should().Be(0);

        // Test by calling UpdateVirtualPage which sets the current page
        paging.UpdateVirtualPage(10, pageSize, null); // This sets current page to 1
        paging.CalculateVirtualStart(pageSize).Should().Be(10);

        paging.UpdateVirtualPage(50, pageSize, null); // This sets current page to 5
        paging.CalculateVirtualStart(pageSize).Should().Be(50);
    }

    [Test]
    public void ShouldEstimateTotalRecordsCorrectly()
    {
        // Arrange
        var paging = new DataTableContinuationTokenPaging();
        var pageSize = 10;

        // Simulate some pages with data
        paging.SetPageRecordCount(0, 10);
        paging.SetPageRecordCount(1, 10);
        paging.SetPageRecordCount(2, 10);
        paging.HasMorePages = true;

        // Act
        int totalRecords = paging.CalculateTotalRecords(pageSize);

        // Assert - Should estimate based on known pages plus some buffer
        totalRecords.Should().BeGreaterThanOrEqualTo(30); // At least the known records
    }
} 
