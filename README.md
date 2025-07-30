[![](https://img.shields.io/nuget/v/soenneker.blazor.datatables.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.blazor.datatables/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blazor.datatables/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.blazor.datatables/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.blazor.datatables.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.blazor.datatables/)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Blazor.DataTables
### A Blazor interop library for DataTables

This library simplifies the integration of DataTables into Blazor applications, providing access to options, events, etc. A demo project showcasing common usages is included.

Diligence was taken to align the Blazor API with JS. Refer to the [DataTables documentation](https://datatables.net/) for details. This is a work-in-progress; contribution is welcomed.

## Installation

```
dotnet add package Soenneker.Blazor.DataTables
```

### Add the following to your `Startup.cs` file

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddDataTablesInteropAsScoped();
}
```

## Usage

```razor
@using Soenneker.Blazor.DataTables

<DataTable Options="_options">
    <thead>
        <tr>
            <th>Name</th>
            <th>Position</th>
            <th>Office</th>
            <th>Age</th>
            <th>Start date</th>
            <th>Salary</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>John Doe</td>
            <td>Developer</td>
            <td>London</td>
            <td>28</td>
            <td>2017/04/25</td>
            <td>$320,800</td>
        </tr>
    </tbody>
</DataTable>

@code{

    private readonly DataTableOptions _options_ = new()
    {
        Searching = true,
        LengthChange = false,
        Info = false,
        Paging = false,
        Order = [new object[] {0, "asc"}]
    };
}
```

## Custom Processing Indicators

When using server-side processing, you can override the default DataTables processing indicator with custom Blazor content. This feature allows you to:

- Display custom Blazor components as the processing indicator
- Use any Blazor markup, components, and styling
- Disable the processing indicator entirely

### Basic Usage

```razor
<DataTable Options="tableOptions" OnServerSideRequest="HandleServerSideRequest">
    <ProcessingIndicator>
        <div class="text-center">
            <div class="spinner-border text-primary" role="status"></div>
            <div class="mt-2">Loading data from server...</div>
        </div>
    </ProcessingIndicator>
    <ChildContent>
        <thead>
            <tr>
                <th data-data="id">ID</th>
                <th data-data="name">Name</th>
                <th data-data="email">Email</th>
            </tr>
        </thead>
        <tbody>
        </tbody>
    </ChildContent>
</DataTable>
```

## Continuation Token Support

The DataTables component supports continuation token-based pagination, which is useful for services like Azure Table Storage, Cosmos DB, or other cloud storage solutions that use continuation tokens instead of traditional offset-based pagination.

### How Continuation Tokens Work

The component automatically emulates traditional DataTables pagination behavior while using continuation tokens under the hood:

1. **First Request**: No continuation token is sent
2. **Server Response**: Server returns data along with a continuation token for the next page
3. **Subsequent Requests**: The continuation token from the previous response is sent with the next request
4. **Last Page**: When there are no more pages, the server returns `null` for the continuation token
5. **Pagination Display**: DataTables shows traditional pagination controls (Previous/Next buttons, page numbers) that work seamlessly with continuation tokens

### Basic Usage

```razor
<DataTable @ref="_dataTable"
           OnServerSideRequest="HandleServerSideRequest"
           Options="_tableOptions">
    <thead>
        <tr>
            <th data-data="id">ID</th>
            <th data-data="name">Name</th>
            <th data-data="email">Email</th>
        </tr>
    </thead>
    <tbody>
    </tbody>
</DataTable>

@code {
    private DataTable _dataTable = null!;
    private readonly DataTableOptions _tableOptions = new()
    {
        ServerSide = true,
        PageLength = 10
    };

    private async Task<DataTableServerResponse> HandleServerSideRequest(DataTableServerSideRequest request)
    {
        // The request.ContinuationToken will contain the token from the previous response
        // or null for the first request
        
        var result = await YourService.GetDataAsync(
            pageSize: request.Length,
            continuationToken: request.ContinuationToken,
            searchTerm: request.Search?.Value,
            orderBy: GetOrderByColumn(request.Order)
        );
        
        // Return the response with the continuation token for the next page
        // Note: The component automatically calculates appropriate TotalRecords and TotalFilteredRecords
        // to make DataTables think it's using traditional pagination
        return DataTableServerResponse.Success(
            draw: request.Draw,
            recordsTotal: result.TotalRecords,
            recordsFiltered: result.TotalFilteredRecords,
            data: result.Data,
            continuationToken: result.NextContinuationToken // null when no more pages
        );
    }
}
```

### Controlling Continuation Tokens

You can programmatically control continuation tokens using the DataTable component methods:

```csharp
// Reset pagination to start from the beginning
_dataTable.ResetContinuationToken();

// Set a specific continuation token for the next request
_dataTable.SetContinuationToken("your-custom-token");
```

### Backward Navigation Support

The component automatically stores continuation tokens for visited pages, enabling backward navigation:

1. **Original Token Storage**: When you navigate to a page, the component stores the original continuation token used to reach that page
2. **Consistent Backward Navigation**: When you click "Previous" or navigate to a previous page, the component uses the same original token for that page
3. **Token Consistency**: Navigating back to a page multiple times will always use the same continuation token that was originally used to reach that page
4. **Efficient Navigation**: If a direct token isn't available for a requested page, the component finds the closest available token and uses it
5. **Seamless UX**: Users can navigate forward and backward through pages just like with traditional pagination

This ensures that your server-side logic receives the same continuation tokens when users navigate back to previously visited pages, maintaining consistency with your data source.

### Implementation Notes

- **Automatic Handling**: The component automatically manages continuation tokens between requests
- **Pagination Emulation**: The component automatically calculates appropriate `TotalRecords` and `TotalFilteredRecords` values to make DataTables think it's using traditional offset-based pagination
- **Virtual Page Tracking**: The component maintains a virtual page state that maps to continuation tokens, allowing DataTables pagination controls to work seamlessly
- **Backward Navigation**: The component stores continuation tokens for visited pages, enabling backward navigation to previous pages
- **Token Storage**: Continuation tokens are stored for each visited page, allowing efficient navigation in both directions
- **Fingerprinting**: The component uses request fingerprinting to detect when search/filter parameters change and automatically resets the continuation token
