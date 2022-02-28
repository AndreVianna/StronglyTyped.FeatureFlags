namespace StronglyTyped.FeatureFlags;

[AttributeUsage(AttributeTargets.Field)]
public class FeatureGroupAttribute : Attribute {
    public FeatureGroupAttribute(params string[] sourcePath) {
        SourcePath = sourcePath;
    }

    public string[] SourcePath { get; }
}