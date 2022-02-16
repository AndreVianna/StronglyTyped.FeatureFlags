using StronglyTyped.FeatureFlags.Abstractions;

namespace StronglyTyped.FeatureFlags.Consumer;

public partial interface IFeatureFlagsAccessor {

}

[FeatureFlagsHolder]
public partial class FeatureFlags : IFeatureFlagsAccessor {
    private static readonly string[] _flags = {
        "SaluteUniverse",
        "OtherFeature"
    };

    private readonly IFeatureFlagsFactory _featureFlagsFactory;

    public FeatureFlags(IFeatureFlagsFactory featureFlagsFactory) {
        _featureFlagsFactory = featureFlagsFactory;
    }

    public IFeatureFlag SaluteUniverse => _featureFlagsFactory.For(nameof(SaluteUniverse));
}
