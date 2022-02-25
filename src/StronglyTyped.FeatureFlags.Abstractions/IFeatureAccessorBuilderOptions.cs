// ReSharper disable once CheckNamespace
namespace StronglyTyped.FeatureFlags;

public interface IFeatureAccessorBuilderOptions {
    void TryAddProvider<TProvider>(Func<IServiceProvider, TProvider>? createProvider = null) where TProvider : class, IFeatureProvider;
}
