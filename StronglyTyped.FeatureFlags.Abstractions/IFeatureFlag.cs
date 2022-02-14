namespace StronglyTyped.FeatureFlags.Abstractions;

public interface IFeatureFlag {
    string Name { get; }
    FeatureFlagType Type { get; }
    bool IsEnabled { get; }
}