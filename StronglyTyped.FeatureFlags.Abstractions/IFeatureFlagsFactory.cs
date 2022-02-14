namespace StronglyTyped.FeatureFlags.Abstractions;

public interface IFeatureFlagsFactory {
    IFeatureFlag For(string featureName);
}