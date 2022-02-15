namespace StronglyTyped.FeatureFlags;

public record FeatureFlag(string Name, FeatureFlagType Type, bool IsEnabled) : IFeatureFlag {
    public static IFeatureFlag GetNullFlagFor(string name) => new FeatureFlag(name, FeatureFlagType.Static, false);
}