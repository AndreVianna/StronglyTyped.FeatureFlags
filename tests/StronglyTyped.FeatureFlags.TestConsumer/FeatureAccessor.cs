namespace StronglyTyped.FeatureFlags.TestConsumer;

[FeatureFlags(nameof(_availableFeatures))]
public partial class FeatureAccessor {
    private const string _someValue = "SomeValue";
    private static readonly string[] _availableFeatures = {
        "SaluteUniverse",
        "OtherFeature",
        _someValue,
        $"Feature{4}"
    };
}
