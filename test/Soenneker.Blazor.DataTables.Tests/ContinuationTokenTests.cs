using Microsoft.Extensions.Logging;
using Moq;
using AwesomeAssertions;
using Soenneker.Blazor.DataTables.Options;
using Soenneker.DataTables.Dtos.ServerSideRequest;
using System.Collections.Generic;
using System;
using Soenneker.DataTables.Dtos.ServerResponse;

namespace Soenneker.Blazor.DataTables.Tests;

public sealed class ContinuationTokenTests
{
    private readonly Mock<ILogger<DataTable>> _mockLogger = new();

    [Test]
    public void DataTableServerResponse_ShouldSupportContinuationToken()
    {
        // Arrange
        var continuationToken = "test-token-123";
        var data = new List<object> { new { Id = 1, Name = "Test User" } };

        // Act
        DataTableServerResponse response = DataTableServerResponse.Success(1, 100, 50, data, continuationToken);

        // Assert
        response.ContinuationToken.Should().Be(continuationToken);
        response.Draw.Should().Be(1);
        response.TotalRecords.Should().Be(100);
        response.TotalFilteredRecords.Should().Be(50);
        response.Data.Should().BeEquivalentTo(data);
    }

    [Test]
    public void DataTableServerResponse_ShouldHandleNullContinuationToken()
    {
        // Arrange
        var data = new List<object> { new { Id = 1, Name = "Test User" } };

        // Act
        DataTableServerResponse response = DataTableServerResponse.Success(1, 100, 50, data, null);

        // Assert
        response.ContinuationToken.Should().BeNull();
        response.Draw.Should().Be(1);
        response.TotalRecords.Should().Be(100);
        response.TotalFilteredRecords.Should().Be(50);
        response.Data.Should().BeEquivalentTo(data);
    }

    [Test]
    public void DataTableServerSideRequest_ShouldSupportContinuationToken()
    {
        // Arrange
        var request = new DataTableServerSideRequest
        {
            Draw = 1,
            Start = 0,
            Length = 10,
            ContinuationToken = "test-token-123",
            Search = new DataTableSearchRequest { Value = "test" },
            Order = new List<DataTableOrderRequest>
            {
                new() { Column = 0, Dir = "asc" }
            }
        };

        // Assert
        request.ContinuationToken.Should().Be("test-token-123");
        request.Draw.Should().Be(1);
        request.Start.Should().Be(0);
        request.Length.Should().Be(10);
        request.Search.Should().NotBeNull();
        request.Order.Should().NotBeNull();
    }

    [Test]
    public void DataTableServerSideRequest_ShouldHandleNullContinuationToken()
    {
        // Arrange
        var request = new DataTableServerSideRequest
        {
            Draw = 1,
            Start = 0,
            Length = 10,
            ContinuationToken = null
        };

        // Assert
        request.ContinuationToken.Should().BeNull();
        request.Draw.Should().Be(1);
        request.Start.Should().Be(0);
        request.Length.Should().Be(10);
    }

    [Test]
    public void DataTableContinuationTokenPaging_ShouldCalculateVirtualStart()
    {
        // Arrange
        var paging = new DataTableContinuationTokenPaging();
        paging.EstimatedTotalRecords = 100;

        // Act
        int virtualStart = paging.CalculateVirtualStart(10);

        // Assert
        virtualStart.Should().Be(0);
    }

    [Test]
    public void DataTableContinuationTokenPaging_ShouldUpdateFromResponse()
    {
        // Arrange
        var paging = new DataTableContinuationTokenPaging();
        var continuationToken = "next-page-token";

        // Act
        paging.UpdateFromResponse(10, 5, continuationToken);

        // Assert
        paging.GetPageRecordCount(0).Should().Be(5);
        paging.GetContinuationToken(1).Should().Be(continuationToken);
        paging.HasMorePages.Should().BeTrue();
    }

    [Test]
    public void DataTableContinuationTokenPaging_ShouldHandleLastPage()
    {
        // Arrange
        var paging = new DataTableContinuationTokenPaging();

        // Act
        paging.UpdateFromResponse(10, 5, null); // null continuation token indicates last page

        // Assert
        paging.GetPageRecordCount(0).Should().Be(5);
        paging.GetContinuationToken(1).Should().BeNull();
        paging.HasMorePages.Should().BeFalse();
    }

    [Test]
    public void DataTableContinuationTokenPaging_ShouldReset()
    {
        // Arrange
        var paging = new DataTableContinuationTokenPaging();
        paging.SetContinuationToken(0, "token");
        paging.SetPageRecordCount(0, 10);
        paging.EstimatedTotalRecords = 100;

        // Act
        paging.Reset();

        // Assert
        paging.GetContinuationToken(0).Should().BeNull();
        paging.GetPageRecordCount(0).Should().Be(0);
        paging.EstimatedTotalRecords.Should().Be(0);
        paging.HasMorePages.Should().BeTrue();
    }

    [Test]
    public void DataTableContinuationTokenPaging_ShouldSupportBackwardNavigation()
    {
        // Arrange
        var paging = new DataTableContinuationTokenPaging();
        
        // Simulate navigating through pages and storing tokens
        paging.SetContinuationToken(0, "token_page_0");
        paging.SetContinuationToken(1, "token_page_1");
        paging.SetContinuationToken(2, "token_page_2");

        // Act - Try to navigate back to page 1
        string? tokenForPage1 = paging.GetBestTokenForPage(1, 10);

        // Assert
        tokenForPage1.Should().Be("token_page_1");
    }

    [Test]
    public void DataTableContinuationTokenPaging_ShouldHandleBackwardNavigationWithClosestToken()
    {
        // Arrange
        var paging = new DataTableContinuationTokenPaging();
        
        // Store tokens for pages 0 and 2, but not page 1
        paging.SetContinuationToken(0, "token_page_0");
        paging.SetContinuationToken(2, "token_page_2");

        // Act - Try to navigate to page 1 (which we don't have a direct token for)
        string? tokenForPage1 = paging.GetBestTokenForPage(1, 10);

        // Assert - Should return the closest token (page 0 in this case)
        tokenForPage1.Should().Be("token_page_0");
    }

    [Test]
    public void DataTableContinuationTokenPaging_ShouldHandleForwardNavigationWithStoredTokens()
    {
        // Arrange
        var paging = new DataTableContinuationTokenPaging();
        
        // Store tokens for pages 0, 1, 2
        paging.SetContinuationToken(0, "token_page_0");
        paging.SetContinuationToken(1, "token_page_1");
        paging.SetContinuationToken(2, "token_page_2");

        // Act - Try to navigate to page 2
        string? tokenForPage2 = paging.GetBestTokenForPage(2, 10);

        // Assert
        tokenForPage2.Should().Be("token_page_2");
    }

    [Test]
    public void DataTableContinuationTokenPaging_ShouldValidateInputs()
    {
        // Arrange
        var paging = new DataTableContinuationTokenPaging();

        // Act & Assert - Test negative page numbers
        Assert.Throws<ArgumentException>(() => paging.GetContinuationToken(-1));
        Assert.Throws<ArgumentException>(() => paging.SetContinuationToken(-1, "token"));
        Assert.Throws<ArgumentException>(() => paging.GetPageRecordCount(-1));
        Assert.Throws<ArgumentException>(() => paging.SetPageRecordCount(-1, 10));
        Assert.Throws<ArgumentException>(() => paging.GetBestTokenForPage(-1, 10));

        // Act & Assert - Test invalid page sizes
        Assert.Throws<ArgumentException>(() => paging.GetBestTokenForPage(1, 0));
        Assert.Throws<ArgumentException>(() => paging.GetBestTokenForPage(1, -1));

        // Act & Assert - Test invalid start positions
        Assert.Throws<ArgumentException>(() => paging.UpdateVirtualPage(-1, 10, null));
        Assert.Throws<ArgumentException>(() => paging.UpdateVirtualPage(0, 0, null));
        Assert.Throws<ArgumentException>(() => paging.UpdateVirtualPage(0, -1, null));

        // Act & Assert - Test negative record counts
        Assert.Throws<ArgumentException>(() => paging.SetPageRecordCount(0, -1));
    }

    [Test]
    public void DataTableContinuationTokenPaging_ShouldHandleEdgeCases()
    {
        // Arrange
        var paging = new DataTableContinuationTokenPaging();

        // Act & Assert - Test with very large page numbers
        paging.SetContinuationToken(int.MaxValue, "large_token");
        paging.GetContinuationToken(int.MaxValue).Should().Be("large_token");

        // Act & Assert - Test with zero page number
        paging.SetContinuationToken(0, "zero_token");
        paging.GetContinuationToken(0).Should().Be("zero_token");

        // Act & Assert - Test with null tokens
        paging.SetContinuationToken(1, null);
        paging.GetContinuationToken(1).Should().BeNull();
    }
} 
