namespace StronglyTyped.FeatureFlags.Abstractions;

public interface IFeatureProvider : IDisposable {
    string Name { get; }
    IFeature GetByName(string featureName);
    IEnumerable<IFeature> GetAll();
}