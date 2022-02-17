using Microsoft.Extensions.DependencyInjection;

namespace StronglyTyped.FeatureFlags.Abstractions;

public interface IFlagsFactoryOptions {
    void AddProvider<TProvider>() where TProvider : class, IFeatureProvider;
}
