// ReSharper disable once CheckNamespace
namespace StronglyTyped.FeatureFlags;

public record Feature(string[] Path, Type ProviderType, FeatureStateLifecycle Lifecycle, bool IsEnabled) : IFeature;