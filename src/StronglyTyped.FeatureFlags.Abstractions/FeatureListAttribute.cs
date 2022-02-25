// ReSharper disable once CheckNamespace
namespace StronglyTyped.FeatureFlags;

[AttributeUsage(AttributeTargets.Class)]
public class FeatureListAttribute : Attribute
{
    public FeatureListAttribute(string fieldName) {
        FieldName = fieldName;
    }

    public string FieldName { get; }
}