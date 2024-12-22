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