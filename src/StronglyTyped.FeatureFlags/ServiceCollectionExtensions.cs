namespace StronglyTyped.FeatureFlags;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddFeatureFlags(this IServiceCollection services, Action<IFeatureReaderBuilderOptions> configure) {
        var builder = new FeatureReaderBuilder(services);
        configure(builder);
        services.TryAddScoped<IFeatureReader>(prv => builder.Build(prv));
        return services;
    }
}
