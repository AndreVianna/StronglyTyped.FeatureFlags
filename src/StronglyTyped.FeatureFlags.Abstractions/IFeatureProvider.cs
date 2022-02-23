// ReSharper disable once CheckNamespace
namespace StronglyTyped.FeatureFlags;

public interface IFeatureProvider : IDisposable {
    IFeature? GetByNameOrDefault(string featureName);
    IEnumerable<IFeature> GetAll();
}