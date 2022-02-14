namespace StronglyTyped.FeatureFlags;

public record FeatureFlag(string Name, FeatureFlagType Type, bool IsEnabled) : IFeatureFlag;