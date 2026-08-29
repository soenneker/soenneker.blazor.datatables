[![](https://img.shields.io/nuget/v/soenneker.blazor.datatables.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.blazor.datatables/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blazor.datatables/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.blazor.datatables/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.blazor.datatables.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.blazor.datatables/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.blazor.datatables/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.blazor.datatables/actions/workflows/codeql.yml)
[![](https://img.shields.io/badge/Demo-Live-blueviolet?style=for-the-badge&logo=github)](https://soenneker.github.io/soenneker.blazor.datatables)

# Soenneker.Blazor.DataTables

A Blazor component and JavaScript interop layer for DataTables, including client-rendered rows, server-side callbacks, custom processing UI, and optional continuation-token paging.

## Installation

```bash
dotnet add package Soenneker.Blazor.DataTables
```

The host application must load DataTables and its chosen styling integration before the component renders. This package does not bundle them. For example:

```html
<link rel="stylesheet"
      href="https://cdn.jsdelivr.net/npm/datatables.net-bs5@3.0.0/css/dataTables.bootstrap5.min.css">
<script src="https://cdn.jsdelivr.net/npm/datatables.net@3.0.0/js/dataTables.min.js"></script>
<script src="https://cdn.jsdelivr.net/npm/datatables.net-bs5@3.0.0/js/dataTables.bootstrap5.min.js"></script>
```

DataTables 3 does not require jQuery. Load compatible major versions for optional DataTables extensions. In production, self-host these assets or apply appropriate integrity and Content Security Policy controls.

Register the scoped interop services:

```csharp
using Soenneker.Blazor.DataTables.Registrars;

builder.Services.AddDataTablesInteropAsScoped();
```

## Client-rendered rows

```razor
@using Soenneker.Blazor.DataTables
@using Soenneker.Blazor.DataTables.Options

<DataTable @ref="_table" Options="_options" class="table table-striped">
    <thead>
        <tr>
            <th>Name</th>
            <th>Office</th>
        </tr>
    </thead>
    <tbody>
        @foreach (Employee employee in _employees)
        {
            <tr @key="employee.Id">
                <td>@employee.Name</td>
                <td>@employee.Office</td>
            </tr>
        }
    </tbody>
</DataTable>

@code {
    private DataTable? _table;
    private readonly List<Employee> _employees = [];

    private readonly DataTableOptions _options = new()
    {
        Searching = true,
        PageLength = 25,
        Order = [new object[] { 0, "asc" }]
    };

    private async Task AddEmployee(Employee employee)
    {
        await _table!.RefreshWithDomUpdate(() => _employees.Add(employee));
    }
}
```

DataTables owns and rearranges the table DOM after initialization. Use `RefreshWithDomUpdate` when row content changes but the table structure and options stay the same. Use `RecreateWithDomUpdate` when columns or initialization options change; it destroys and rebuilds the JavaScript instance.

## Offset-based server-side data

Leave `UseContinuationTokenPaging` disabled for a normal offset/limit backend:

```razor
<DataTable Options="_serverOptions"
           OnServerSideRequest="LoadPage">
    <thead>
        <tr>
            <th data-data="id">ID</th>
            <th data-data="name">Name</th>
            <th data-data="createdAt">Created</th>
        </tr>
    </thead>
    <tbody></tbody>
</DataTable>

@code {
    private readonly DataTableOptions _serverOptions = new()
    {
        ServerSide = true,
        Processing = true,
        PageLength = 25
    };

    private async Task<DataTableServerResponse> LoadPage(
        DataTableServerSideRequest request)
    {
        string orderBy = request.Order?.FirstOrDefault()?.Column switch
        {
            0 => "id",
            1 => "name",
            2 => "created_at",
            _ => "id"
        };

        PageResult<RowDto> page = await repository.LoadPage(
            offset: request.Start,
            limit: request.Length,
            search: request.Search?.Value,
            orderBy: orderBy);

        return DataTableServerResponse.Success(
            draw: request.Draw,
            recordsTotal: page.Total,
            recordsFiltered: page.FilteredTotal,
            data: page.Items);
    }
}
```

Always return `request.Draw`; DataTables uses it to discard out-of-order responses. Treat column indices, direction values, search text, and page sizes as untrusted input. Map order columns through an allowlist, cap page/search sizes, and use parameterized queries—never concatenate request values into SQL. Encode untrusted cell content or configure a text renderer instead of returning executable HTML.

## Continuation-token paging

Enable the adapter explicitly for a backend that returns opaque next-page tokens:

```csharp
private readonly DataTableOptions _options = new()
{
    ServerSide = true,
    UseContinuationTokenPaging = true,
    PagingType = "simple",
    PageLength = 25
};

private async Task<DataTableServerResponse> LoadTokenPage(
    DataTableServerSideRequest request)
{
    TokenPage<RowDto> page = await repository.LoadNext(
        request.ContinuationToken,
        request.Length,
        request.Search?.Value);

    return DataTableServerResponse.Success(
        draw: request.Draw,
        recordsTotal: page.Total ?? 0,
        recordsFiltered: page.FilteredTotal ?? 0,
        data: page.Items,
        continuationToken: page.NextToken);
}
```

Use a previous/next pager such as `PagingType = "simple"`: an opaque token cannot jump directly to an unvisited page. Tokens for visited pages are retained for backward navigation. Search, ordering, column filters, or page-length changes reset token state. Call `ResetContinuationToken()` after external filters or the underlying dataset changes; `SetContinuationToken(token)` overrides the token used for the next request.

When the backend supplies positive total counts, they are preserved. Otherwise the adapter estimates counts so DataTables can keep paging until a response has no next token.

## Custom processing content

Provide `ProcessingIndicator` to replace DataTables' processing element during server-side requests:

```razor
<DataTable Options="_serverOptions" OnServerSideRequest="LoadPage">
    <ProcessingIndicator>
        <div role="status">Loading…</div>
    </ProcessingIndicator>
    <ChildContent>
        <!-- thead and tbody -->
    </ChildContent>
</DataTable>
```

`OnInitialize` fires after DataTables reports initialization. `OnDestroy` fires during component disposal. The component destroys its JavaScript table, mutation observer, and .NET callback reference when removed.
