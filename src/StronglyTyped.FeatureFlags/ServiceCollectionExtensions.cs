﻿namespace StronglyTyped.FeatureFlags;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddFeatureFlags(this IServiceCollection services, Action<IFlagsFactoryOptions> configure) {
        var factory = new FeatureFlagsFactory(services);
        configure(factory);
        services.AddSingleton<IFlagsFactory>(factory);
        return services;
    }
}
