using System.Collections.Generic;
using System;

namespace Soenneker.Blazor.DataTables.Options;

/// <summary>
/// Helper class to manage continuation token pagination that emulates traditional DataTables pagination behavior.
/// This allows you to use continuation tokens while making DataTables think it's using offset-based pagination.
/// </summary>
public class DataTableContinuationTokenPaging
{
    private readonly Dictionary<string, string> _pageTokens = new();
    private readonly Dictionary<string, int> _pageCounts = new();
    private int _currentVirtualPage = 0;
    private int _estimatedTotalRecords = 0;
    private bool _hasMorePages = true;

    /// <summary>
    /// Gets or sets the current virtual page number (0-based).
    /// </summary>
    public int CurrentVirtualPage => _currentVirtualPage;

    /// <summary>
    /// Gets or sets the estimated total number of records.
    /// This is used to calculate pagination info for DataTables.
    /// </summary>
    public int EstimatedTotalRecords
    {
        get => _estimatedTotalRecords;
        set => _estimatedTotalRecords = value;
    }

    /// <summary>
    /// Gets or sets whether there are more pages available.
    /// </summary>
    public bool HasMorePages
    {
        get => _hasMorePages;
        set => _hasMorePages = value;
    }

    /// <summary>
    /// Gets the current continuation token for the specified page.
    /// </summary>
    /// <param name="pageNumber">The page number (0-based).</param>
    /// <returns>The continuation token for the page, or null if not found.</returns>
    public string? GetContinuationToken(int pageNumber)
    {
        // Validate input
        if (pageNumber < 0)
        {
            throw new ArgumentException("pageNumber must be non-negative", nameof(pageNumber));
        }

        if (_pageTokens.TryGetValue(pageNumber.ToString(), out string? token))
        {
            // Return null for empty strings (which represent null tokens)
            return string.IsNullOrEmpty(token) ? null : token;
        }
        return null;
    }

    /// <summary>
    /// Sets the continuation token for a specific page.
    /// </summary>
    /// <param name="pageNumber">The page number (0-based).</param>
    /// <param name="continuationToken">The continuation token for the page.</param>
    public void SetContinuationToken(int pageNumber, string? continuationToken)
    {
        // Validate input
        if (pageNumber < 0)
        {
            throw new ArgumentException("pageNumber must be non-negative", nameof(pageNumber));
        }

        if (continuationToken == null)
        {
            // Store null tokens as empty string to distinguish from missing tokens
            _pageTokens[pageNumber.ToString()] = string.Empty;
        }
        else
        {
            _pageTokens[pageNumber.ToString()] = continuationToken;
        }
    }

    /// <summary>
    /// Gets the number of records returned for a specific page.
    /// </summary>
    /// <param name="pageNumber">The page number (0-based).</param>
    /// <returns>The number of records returned for the page.</returns>
    public int GetPageRecordCount(int pageNumber)
    {
        // Validate input
        if (pageNumber < 0)
        {
            throw new ArgumentException("pageNumber must be non-negative", nameof(pageNumber));
        }

        return _pageCounts.GetValueOrDefault(pageNumber.ToString(), 0);
    }

    /// <summary>
    /// Sets the number of records returned for a specific page.
    /// </summary>
    /// <param name="pageNumber">The page number (0-based).</param>
    /// <param name="recordCount">The number of records returned for the page.</param>
    public void SetPageRecordCount(int pageNumber, int recordCount)
    {
        // Validate inputs
        if (pageNumber < 0)
        {
            throw new ArgumentException("pageNumber must be non-negative", nameof(pageNumber));
        }

        if (recordCount < 0)
        {
            throw new ArgumentException("recordCount must be non-negative", nameof(recordCount));
        }

        _pageCounts[pageNumber.ToString()] = recordCount;
    }

    /// <summary>
    /// Calculates the virtual start position for DataTables pagination.
    /// </summary>
    /// <param name="pageSize">The page size.</param>
    /// <returns>The virtual start position.</returns>
    public int CalculateVirtualStart(int pageSize)
    {
        return _currentVirtualPage * pageSize;
    }

    /// <summary>
    /// Calculates the total number of records for DataTables pagination.
    /// This is an estimate based on available data.
    /// </summary>
    /// <param name="pageSize">The page size.</param>
    /// <returns>The estimated total number of records.</returns>
    public int CalculateTotalRecords(int pageSize)
    {
        if (_estimatedTotalRecords > 0)
        {
            return _estimatedTotalRecords;
        }

        // If we don't have an estimated total, calculate based on known pages
        var knownRecords = 0;
        int maxPage = -1;

        foreach (KeyValuePair<string, int> kvp in _pageCounts)
        {
            if (int.TryParse(kvp.Key, out int pageNum))
            {
                knownRecords += kvp.Value;
                maxPage = Math.Max(maxPage, pageNum);
            }
        }

        // If we have more pages, estimate the total
        if (_hasMorePages && maxPage >= 0)
        {
            // Estimate based on average records per page
            int avgRecordsPerPage = knownRecords / (maxPage + 1);
            return Math.Max(knownRecords + avgRecordsPerPage, (maxPage + 2) * pageSize);
        }

        return knownRecords;
    }

    /// <summary>
    /// Calculates the filtered records count for DataTables pagination.
    /// This is typically the same as total records for continuation token systems.
    /// </summary>
    /// <returns>The filtered records count.</returns>
    public int CalculateFilteredRecords()
    {
        return CalculateTotalRecords(10); // Use a default page size for calculation
    }

    /// <summary>
    /// Updates the current virtual page and manages pagination state.
    /// </summary>
    /// <param name="requestedStart">The start position requested by DataTables.</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="continuationToken">The continuation token from the previous response.</param>
    /// <returns>The continuation token to use for the current request.</returns>
    public string? UpdateVirtualPage(int requestedStart, int pageSize, string? continuationToken)
    {
        // Validate inputs
        if (requestedStart < 0 || pageSize <= 0)
        {
            throw new ArgumentException("requestedStart must be non-negative and pageSize must be positive");
        }

        int requestedPage = requestedStart / pageSize;

        // For the first page (page 0), always return null as the continuation token
        if (requestedPage == 0)
        {
            _currentVirtualPage = requestedPage;
            return null;
        }

        // First, check if we have a direct token for the requested page
        string? directToken = GetContinuationToken(requestedPage);
        if (!string.IsNullOrEmpty(directToken))
        {
            _currentVirtualPage = requestedPage;
            return directToken;
        }

        // If we have a continuation token from the previous response and no direct token for the requested page,
        // use the previous token (this handles forward navigation)
        if (!string.IsNullOrEmpty(continuationToken))
        {
            _currentVirtualPage = requestedPage;
            return continuationToken;
        }

        // Use the best available token for the requested page (this handles backward navigation to unknown pages)
        string? bestToken = GetBestTokenForPage(requestedPage, pageSize);
        _currentVirtualPage = requestedPage;
        return bestToken;
    }

    /// <summary>
    /// Resets the pagination state.
    /// </summary>
    public void Reset()
    {
        _pageTokens.Clear();
        _pageCounts.Clear();
        _currentVirtualPage = 0;
        _estimatedTotalRecords = 0;
        _hasMorePages = true;
    }

    /// <summary>
    /// Updates the pagination state with the response data.
    /// </summary>
    /// <param name="pageSize">The page size.</param>
    /// <param name="recordCount">The number of records in the current response.</param>
    /// <param name="continuationToken">The continuation token for the next page.</param>
    /// <param name="tokenUsedForCurrentPage">The continuation token that was used to reach the current page.</param>
    public void UpdateFromResponse(int pageSize, int recordCount, string? continuationToken, string? tokenUsedForCurrentPage = null)
    {
        SetPageRecordCount(_currentVirtualPage, recordCount);

        // Store the token that was used for the current page (for backward navigation)
        if (!string.IsNullOrEmpty(tokenUsedForCurrentPage))
        {
            SetContinuationToken(_currentVirtualPage, tokenUsedForCurrentPage);
        }

        // Store the continuation token from the response for the next page
        // This allows us to navigate to the next page using this token
        if (!string.IsNullOrEmpty(continuationToken))
        {
            SetContinuationToken(_currentVirtualPage + 1, continuationToken);
            _hasMorePages = true;
        }
        else
        {
            _hasMorePages = false;
        }

        // Update estimated total if we have more data
        if (_hasMorePages)
        {
            var knownRecords = 0;
            foreach (int count in _pageCounts.Values)
            {
                knownRecords += count;
            }
            _estimatedTotalRecords = Math.Max(_estimatedTotalRecords, knownRecords + pageSize);
        }
    }



    /// <summary>
    /// Gets the best continuation token to use for navigating to the requested page.
    /// This method tries to find the most efficient path to the requested page.
    /// </summary>
    /// <param name="requestedPage">The page number to navigate to (0-based).</param>
    /// <param name="pageSize">The page size.</param>
    /// <returns>The best continuation token to use, or null if starting from the beginning.</returns>
    public string? GetBestTokenForPage(int requestedPage, int pageSize)
    {
        // Validate inputs
        if (requestedPage < 0)
        {
            throw new ArgumentException("requestedPage must be non-negative", nameof(requestedPage));
        }

        if (pageSize <= 0)
        {
            throw new ArgumentException("pageSize must be positive", nameof(pageSize));
        }

        // If we have a direct token for the requested page, use it
        string? directToken = GetContinuationToken(requestedPage);

        if (directToken != null)
        {
            return directToken;
        }

        // If the requested page has a stored null token, return null
        if (_pageTokens.ContainsKey(requestedPage.ToString()))
        {
            return null;
        }

        // Find the closest known page that we can navigate from
        int closestPage = FindClosestPage(requestedPage);

        if (closestPage >= 0)
        {
            string? closestPageToken = GetContinuationToken(closestPage);

            // If the closest page has a null token, return null
            return closestPageToken;
        }

        // No known pages, start from beginning
        return null;
    }

    /// <summary>
    /// Finds the closest page to the requested page that has a stored token.
    /// </summary>
    /// <param name="requestedPage">The page number to find the closest page for.</param>
    /// <returns>The closest page number, or -1 if no pages are found.</returns>
    private int FindClosestPage(int requestedPage)
    {
        int closestPage = -1;
        var minDistance = int.MaxValue;

        foreach (KeyValuePair<string, string> kvp in _pageTokens)
        {
            if (int.TryParse(kvp.Key, out int pageNum))
            {
                int distance = Math.Abs(pageNum - requestedPage);
                
                if (distance < minDistance || (distance == minDistance && pageNum < closestPage))
                {
                    minDistance = distance;
                    closestPage = pageNum;
                }
            }
        }

        return closestPage;
    }
} 