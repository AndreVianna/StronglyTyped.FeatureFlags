namespace StronglyTyped.FeatureFlags.TestConsumer;

[FeatureList(nameof(_availableFeatures))]
public partial class Features {
    private const string _someValue = "SomeValue";
    private static readonly string[] _availableFeatures = {
        "SaluteUniverse",
        "OtherFeature",
        _someValue,
        $"Feature{4}"
    };
}
