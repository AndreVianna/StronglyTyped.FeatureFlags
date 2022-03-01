// ReSharper disable once CheckNamespace
namespace StronglyTyped.FeatureFlags;

[AttributeUsage(AttributeTargets.Field)]
public class FeaturesAttribute : Attribute {
    public FeaturesAttribute(params string[] path) {
        Path = path;
    }

    public string[] Path { get; }
}