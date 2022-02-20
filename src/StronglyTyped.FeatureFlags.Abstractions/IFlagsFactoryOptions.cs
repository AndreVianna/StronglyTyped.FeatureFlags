namespace StronglyTyped.FeatureFlags;

public interface IFlagsFactoryOptions {
    void AddProvider<TProvider>() where TProvider : class, IFeatureProvider;
    void AddProvider<TProvider>(Func<IServiceProvider, TProvider> createProvider) where TProvider : class, IFeatureProvider;
}
