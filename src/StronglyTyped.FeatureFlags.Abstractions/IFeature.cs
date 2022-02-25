// ReSharper disable once CheckNamespace
namespace StronglyTyped.FeatureFlags;

public interface IFeature : IFeatureState {
    string Name { get; }
    Type ProviderType { get; }
    FeatureStateLifecycle Lifecycle { get; }
}