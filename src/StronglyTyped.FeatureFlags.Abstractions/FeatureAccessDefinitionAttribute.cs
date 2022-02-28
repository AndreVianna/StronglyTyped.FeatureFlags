// ReSharper disable once CheckNamespace
namespace StronglyTyped.FeatureFlags;

[AttributeUsage(AttributeTargets.Class)]
public class FeatureAccessDefinitionAttribute : Attribute
{
    public FeatureAccessDefinitionAttribute(params string[] basePath) {
        BasePath = basePath;
    }

    public string[] BasePath { get; }
}