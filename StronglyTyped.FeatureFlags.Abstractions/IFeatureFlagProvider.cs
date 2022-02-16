namespace StronglyTyped.FeatureFlags.Abstractions;

public interface IFeatureFlagProvider : IDisposable {
    string Name { get; }
    IFeatureFlagEntity GetByName(string featureName);
    IEnumerable<IFeatureFlagEntity> GetAll();
}