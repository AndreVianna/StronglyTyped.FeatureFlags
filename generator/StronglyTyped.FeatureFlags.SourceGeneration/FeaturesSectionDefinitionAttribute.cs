// ReSharper disable once CheckNamespace
namespace StronglyTyped.FeatureFlags;

[AttributeUsage(AttributeTargets.Class)]
public class FeaturesSectionDefinitionAttribute : Attribute
{
    public FeaturesSectionDefinitionAttribute(params string[] basePath) {
        BasePath = basePath;
    }

    public string[] BasePath { get; }
}