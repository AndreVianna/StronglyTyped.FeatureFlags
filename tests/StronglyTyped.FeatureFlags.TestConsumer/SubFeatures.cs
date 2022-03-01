namespace StronglyTyped.FeatureFlags.TestConsumer;

[FeatureAccessDefinition]
public partial class SubFeatures {
    [FeatureGroup]
    private static readonly string[] _availableFeatures = {
        "Feature1",
        "Feature2",
    };
}
