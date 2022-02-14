namespace StronglyTyped.FeatureFlags.Abstractions;

public interface IFeatureFlagProvider : IDisposable {
    string Name { get; }
    IFeatureFlag GetByName(string featureName);
    IEnumerable<IFeatureFlag> GetAll();
}