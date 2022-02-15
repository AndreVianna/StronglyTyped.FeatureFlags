using Microsoft.Extensions.DependencyInjection;

namespace StronglyTyped.FeatureFlags;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddFeatureFlags(this IServiceCollection services, Action<IFeatureFlagsOptions> registerProviders) {
        var factory = new FeatureFlagsFactory(services);
        registerProviders(factory);
        services.AddSingleton<IFeatureFlagsFactory>(_ => factory);
        return services;
    }
}
