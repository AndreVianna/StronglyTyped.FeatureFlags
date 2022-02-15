using Microsoft.Extensions.DependencyInjection;

namespace StronglyTyped.FeatureFlags.Abstractions;

public interface IFeatureFlagsOptions {
    void AddProvider<TProvider>() where TProvider : class, IFeatureFlagProvider;
}
