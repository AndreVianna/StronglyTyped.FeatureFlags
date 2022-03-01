namespace StronglyTyped.FeatureFlags.TestConsumer;

[FeaturesSectionDefinition]
public partial class SubFeatures {
    [Features]
    private static readonly string[] _features = {
        "Feature1",
        "Feature2",
    };
}
