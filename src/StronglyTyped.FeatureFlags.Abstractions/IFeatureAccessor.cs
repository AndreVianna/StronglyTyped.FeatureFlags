// ReSharper disable once CheckNamespace
namespace StronglyTyped.FeatureFlags;

public interface IFeatureAccessor {
    IFeatureState For(string featureName);
}