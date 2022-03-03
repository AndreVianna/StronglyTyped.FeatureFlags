// ReSharper disable once CheckNamespace
namespace StronglyTyped.FeatureFlags;

public interface IFeature : IFeatureState {
    string[] Path { get; }
    Type ProviderType { get; }
    FeatureStateLifecycle Lifecycle { get; }
}