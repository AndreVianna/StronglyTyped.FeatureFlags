// ReSharper disable once CheckNamespace
namespace StronglyTyped.FeatureFlags;

public record Feature(string Name, FlagType Type, bool IsEnabled) : IFeature;