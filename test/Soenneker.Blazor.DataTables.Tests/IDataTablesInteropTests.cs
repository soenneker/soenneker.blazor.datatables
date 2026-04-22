using Soenneker.Blazor.DataTables.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Blazor.DataTables.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class DataTablesInteropTests : HostedUnitTest
{
    private readonly IDataTablesInterop _util;

    public DataTablesInteropTests(Host host) : base(host)
    {
        _util = Resolve<IDataTablesInterop>(true);
    }

    [Test]
    public void Default()
    {

    }
}
