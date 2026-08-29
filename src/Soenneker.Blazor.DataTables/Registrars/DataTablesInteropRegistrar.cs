using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Blazor.DataTables.Abstract;
using Soenneker.Blazor.Utils.InteropEventListener.Registrars;
using Soenneker.Blazor.Utils.ResourceLoader.Registrars;

namespace Soenneker.Blazor.DataTables.Registrars;

/// <summary>
/// A Blazor interop library for DataTables
/// </summary>
public static class DataTablesInteropRegistrar
{
    /// <summary>
    /// Adds <see cref="IDataTablesInterop"/> as a scoped service. <para/>
    /// </summary>
    /// <param name="services">Service collection that receives the registration.</param>
    /// <returns>The same service collection, so additional registrations can be chained.</returns>
    public static IServiceCollection AddDataTablesInteropAsScoped(this IServiceCollection services)
    {
        services.AddResourceLoaderAsScoped()
                .AddInteropEventListenerAsScoped()
                .TryAddScoped<IDataTablesInterop, DataTablesInterop>();

        return services;
    }
}
