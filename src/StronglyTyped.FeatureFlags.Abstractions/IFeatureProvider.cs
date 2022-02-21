// ReSharper disable once CheckNamespace
namespace StronglyTyped.FeatureFlags;

public interface IFeatureProvider : IDisposable {
    string Name { get; }
    IFeature? GetByNameOrDefault(string featureName);
    IEnumerable<IFeature> GetAll();
}