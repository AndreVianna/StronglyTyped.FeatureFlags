namespace StronglyTyped.FeatureFlags;

public interface IFeatureProvider : IDisposable {
    string Name { get; }
    IFeature GetByName(string featureName);
    IEnumerable<IFeature> GetAll();
}