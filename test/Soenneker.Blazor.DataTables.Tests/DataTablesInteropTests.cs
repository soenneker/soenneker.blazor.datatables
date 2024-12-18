using Soenneker.Blazor.DataTables.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;


namespace Soenneker.Blazor.DataTables.Tests;

[Collection("Collection")]
public class DataTablesInteropTests : FixturedUnitTest
{
    private readonly IDataTablesInterop _interop;

    public DataTablesInteropTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _interop = Resolve<IDataTablesInterop>(true);
    }
}
