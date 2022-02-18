namespace StronglyTyped.FeatureFlags;

public interface IFlagsFactoryOptions {
    void AddProvider<TProvider>() where TProvider : class, IFeatureProvider;
}
