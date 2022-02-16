namespace StronglyTyped.FeatureFlags;

public record Feature(string Name, FlagType Type, bool IsEnabled) : IFeature {
    public static IFeature GetNullFlagFor(string name) => new Feature(name, FlagType.Static, false);
}