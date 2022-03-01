namespace StronglyTyped.FeatureFlags.TestConsumer;

[FeaturesSectionDefinition]
public partial class Features {
    [Features]
    private static readonly string[] _features = {
        "SaluteUniverse",
        "OtherFeature",
    };
    [Features]
    private static readonly string[] _moreFeatures = {
        "SaluteUniverse2",
        "OtherFeature2",
    };
    [Sections]
    private static readonly string[] _sections = {
        nameof(SubFeatures)
    };
}
