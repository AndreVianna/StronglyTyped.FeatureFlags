namespace StronglyTyped.FeatureFlags;

public record FeatureFlag(string Name, FeatureFlagType Type, bool IsEnabled) : IFeatureFlagEntity {
    public static IFeatureFlagEntity GetNullFlagFor(string name) => new FeatureFlag(name, FeatureFlagType.Static, false);
}