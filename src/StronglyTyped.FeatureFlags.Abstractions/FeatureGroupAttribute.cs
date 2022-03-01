namespace StronglyTyped.FeatureFlags;

[AttributeUsage(AttributeTargets.Field)]
public class FeatureGroupAttribute : Attribute {
    public FeatureGroupAttribute(params string[] groupPath) {
        GroupPath = groupPath;
    }

    public string[] GroupPath { get; }
}