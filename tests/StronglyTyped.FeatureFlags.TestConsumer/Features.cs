namespace StronglyTyped.FeatureFlags.TestConsumer;

[FeatureAccessDefinition]
public partial class Features {
    [FeatureGroup]
    private static readonly string[] _availableFeatures = {
        "SaluteUniverse",
        "OtherFeature",
        nameof(SubFeatures)
    };
    [FeatureGroup]
    private static readonly string[] _availableFeatures2 = {
        "SaluteUniverse2",
        "OtherFeature2",
    };
}
