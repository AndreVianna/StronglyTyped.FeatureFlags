namespace StronglyTyped.FeatureFlags;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddFeatureFlags(this IServiceCollection services, Action<IFlagsFactoryOptions> configure) {
        var builder = new FeatureFlagsFactoryBuilder(services);
        configure(builder);
        services.TryAddScoped<IFlagsFactory>(prv => builder.Build(prv));
        return services;
    }
}
