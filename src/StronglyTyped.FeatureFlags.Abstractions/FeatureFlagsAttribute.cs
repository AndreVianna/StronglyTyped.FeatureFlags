// ReSharper disable once CheckNamespace
namespace StronglyTyped.FeatureFlags;

[AttributeUsage(AttributeTargets.Class)]
public class FeatureFlagsAttribute : Attribute
{
    public FeatureFlagsAttribute(string fieldName) {
        FieldName = fieldName;
    }

    public string FieldName { get; }
}