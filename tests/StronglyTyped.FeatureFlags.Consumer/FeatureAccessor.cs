using System.Diagnostics.CodeAnalysis;

namespace StronglyTyped.FeatureFlags.Consumer;

[FeatureFlagsSelector]
public partial class FeatureAccessor {
    [SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Used for code generation.")]
    private static readonly string[] _availableFeatures = {
        "SaluteUniverse",
        "OtherFeature"
    };
}
