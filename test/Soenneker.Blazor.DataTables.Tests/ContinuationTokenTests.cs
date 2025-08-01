using Microsoft.Extensions.Logging;
using Moq;
using Soenneker.Blazor.DataTables.Options;
using Soenneker.DataTables.Dtos.ServerSideRequest;
using Xunit;
using System.Collections.Generic;
using System;
using Soenneker.DataTables.Dtos.ServerResponse;

namespace Soenneker.Blazor.DataTables.Tests;

public sealed class ContinuationTokenTests
{
    private readonly Mock<ILogger<DataTable>> _mockLogger = new();

    [Fact]
    public void DataTableServerResponse_ShouldSupportContinuationToken()
    {
        // Arrange
        var continuationToken = "test-token-123";
        var data = new List<object> { new { Id = 1, Name = "Test User" } };

        // Act
        DataTableServerResponse response = DataTableServerResponse.Success(1, 100, 50, data, continuationToken);

        // Assert
        Assert.Equal(continuationToken, response.ContinuationToken);
        Assert.Equal(1, response.Draw);
        Assert.Equal(100, response.TotalRecords);
        Assert.Equal(50, response.TotalFilteredRecords);
        Assert.Equal(data, response.Data);
    }

    [Fact]
    public void DataTableServerResponse_ShouldHandleNullContinuationToken()
    {
        // Arrange
        var data = new List<object> { new { Id = 1, Name = "Test User" } };

        // Act
        DataTableServerResponse response = DataTableServerResponse.Success(1, 100, 50, data, null);

        // Assert
        Assert.Null(response.ContinuationToken);
        Assert.Equal(1, response.Draw);
        Assert.Equal(100, response.TotalRecords);
        Assert.Equal(50, response.TotalFilteredRecords);
        Assert.Equal(data, response.Data);
    }

    [Fact]
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
        Assert.Equal("test-token-123", request.ContinuationToken);
        Assert.Equal(1, request.Draw);
        Assert.Equal(0, request.Start);
        Assert.Equal(10, request.Length);
        Assert.NotNull(request.Search);
        Assert.NotNull(request.Order);
    }

    [Fact]
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
        Assert.Null(request.ContinuationToken);
        Assert.Equal(1, request.Draw);
        Assert.Equal(0, request.Start);
        Assert.Equal(10, request.Length);
    }

    [Fact]
    public void DataTableContinuationTokenPaging_ShouldCalculateVirtualStart()
    {
        // Arrange
        var paging = new DataTableContinuationTokenPaging();
        paging.EstimatedTotalRecords = 100;

        // Act
        int virtualStart = paging.CalculateVirtualStart(10);

        // Assert
        Assert.Equal(0, virtualStart);
    }

    [Fact]
    public void DataTableContinuationTokenPaging_ShouldUpdateFromResponse()
    {
        // Arrange
        var paging = new DataTableContinuationTokenPaging();
        var continuationToken = "next-page-token";

        // Act
        paging.UpdateFromResponse(10, 5, continuationToken);

        // Assert
        Assert.Equal(5, paging.GetPageRecordCount(0));
        Assert.Equal(continuationToken, paging.GetContinuationToken(1));
        Assert.True(paging.HasMorePages);
    }

    [Fact]
    public void DataTableContinuationTokenPaging_ShouldHandleLastPage()
    {
        // Arrange
        var paging = new DataTableContinuationTokenPaging();

        // Act
        paging.UpdateFromResponse(10, 5, null); // null continuation token indicates last page

        // Assert
        Assert.Equal(5, paging.GetPageRecordCount(0));
        Assert.Null(paging.GetContinuationToken(1));
        Assert.False(paging.HasMorePages);
    }

    [Fact]
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
        Assert.Null(paging.GetContinuationToken(0));
        Assert.Equal(0, paging.GetPageRecordCount(0));
        Assert.Equal(0, paging.EstimatedTotalRecords);
        Assert.True(paging.HasMorePages);
    }

    [Fact]
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
        Assert.Equal("token_page_1", tokenForPage1);
    }

    [Fact]
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
        Assert.Equal("token_page_0", tokenForPage1);
    }

    [Fact]
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
        Assert.Equal("token_page_2", tokenForPage2);
    }

    [Fact]
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

    [Fact]
    public void DataTableContinuationTokenPaging_ShouldHandleEdgeCases()
    {
        // Arrange
        var paging = new DataTableContinuationTokenPaging();

        // Act & Assert - Test with very large page numbers
        paging.SetContinuationToken(int.MaxValue, "large_token");
        Assert.Equal("large_token", paging.GetContinuationToken(int.MaxValue));

        // Act & Assert - Test with zero page number
        paging.SetContinuationToken(0, "zero_token");
        Assert.Equal("zero_token", paging.GetContinuationToken(0));

        // Act & Assert - Test with null tokens
        paging.SetContinuationToken(1, null);
        Assert.Null(paging.GetContinuationToken(1));
    }
} 