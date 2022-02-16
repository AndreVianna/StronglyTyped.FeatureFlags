namespace StronglyTyped.FeatureFlags.Abstractions;

public interface IFeatureFlagEntity : IFeatureFlag {
    string Name { get; }
    FeatureFlagType Type { get; }
}