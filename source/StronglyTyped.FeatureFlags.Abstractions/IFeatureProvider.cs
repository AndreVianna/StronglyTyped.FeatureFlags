// ReSharper disable once CheckNamespace
namespace StronglyTyped.FeatureFlags;

public interface IFeatureProvider : IDisposable {
    IFeature? GetFromPathOrDefault(params string[] path);
    IEnumerable<IFeature> GetAll();
}