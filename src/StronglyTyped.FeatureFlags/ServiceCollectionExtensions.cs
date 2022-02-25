namespace StronglyTyped.FeatureFlags;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddFeatureFlags(this IServiceCollection services, Action<IFeatureAccessorBuilderOptions> configure) {
        var builder = new FeatureAccessorBuilder(services);
        configure(builder);
        services.TryAddScoped<IFeatureAccessor>(prv => builder.Build(prv));
        return services;
    }
}
