using Soenneker.Blazor.DataTables.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.Blazor.DataTables.Tests;

[Collection("Collection")]
public class DataTablesInteropTests : FixturedUnitTest
{
    private readonly IDataTablesInterop _util;

    public DataTablesInteropTests(DataFixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _util = Resolve<IDataTablesInterop>(true);
    }

    [Fact]
    public void Default()
    {

    }
}
