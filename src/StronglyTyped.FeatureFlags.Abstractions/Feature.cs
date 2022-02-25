// ReSharper disable once CheckNamespace
namespace StronglyTyped.FeatureFlags;

public record Feature(string Name, Type ProviderType, FeatureStateLifecycle Lifecycle, bool IsEnabled) : IFeature;