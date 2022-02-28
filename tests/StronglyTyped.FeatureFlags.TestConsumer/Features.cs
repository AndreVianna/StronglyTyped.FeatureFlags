namespace StronglyTyped.FeatureFlags.TestConsumer;

[FeatureAccessDefinition(nameof(_availableFeatures))]
public partial class Features {
    private static readonly string[] _availableFeatures = {
        "SaluteUniverse",
        "OtherFeature",
        nameof(SubFeatures)
    };
}
