using System.Diagnostics.CodeAnalysis;
using BlazorSlice.Toast.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BlazorSlice.Toast.Extensions;

[ExcludeFromCodeCoverage]
public static class HostingExtensions
{
    public static IServiceCollection AddBlazorSliceToast(this IServiceCollection services, ToastConfiguration configuration = null)
    {
        configuration ??= new ToastConfiguration();
        services.TryAddScoped<IToast>(builder => new ToastService(builder.GetService<NavigationManager>(), configuration));
        return services;
    }
    
    public static IServiceCollection AddBlazorSliceToast(this IServiceCollection services, Action<ToastConfiguration> configuration)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        var options = new ToastConfiguration();
        configuration(options);
        return AddBlazorSliceToast(services, options);
    }
}