namespace StronglyTyped.FeatureFlags;

public record FeatureFlag(string Name, FeatureFlagType Type, string Provider, bool IsEnabled) : FeatureFlagDescriptor(Name, Type, Provider), IFeatureFlag {
    public static IFeatureFlag GetNullFlagFor(string name) => new FeatureFlag(name, FeatureFlagType.Static, string.Empty, false);
}