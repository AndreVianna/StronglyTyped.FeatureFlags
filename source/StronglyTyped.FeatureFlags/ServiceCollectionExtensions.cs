namespace StronglyTyped.FeatureFlags;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddFeatureFlagsFrom<TAssemblyLocator>(this IServiceCollection services, Action<IFeatureReaderBuilderOptions> configure) {
        var builder = new FeatureReaderBuilder(services);
        configure(builder);
        services.TryAddScoped<IFeatureReader>(prv => builder.Build(prv));

        var assembly = typeof(TAssemblyLocator).Assembly;
        var types = assembly.GetTypes().Where(t => t.GetCustomAttribute<FeaturesSectionDefinitionAttribute>() is not null).ToArray();
        foreach (var type in types) {
            var @interface = type.GetInterface($"I{type.Name}");
            if (@interface == null) continue;
            services.AddScoped(@interface, type);
        }
        return services;
    }
}
