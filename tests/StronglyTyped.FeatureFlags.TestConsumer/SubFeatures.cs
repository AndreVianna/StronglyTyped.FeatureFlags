namespace StronglyTyped.FeatureFlags.TestConsumer;

[FeatureAccessDefinition(nameof(_availableFeatures))]
public partial class SubFeatures {
    private static readonly string[] _availableFeatures = {
        "Feature1",
        "Feature2",
    };
}
