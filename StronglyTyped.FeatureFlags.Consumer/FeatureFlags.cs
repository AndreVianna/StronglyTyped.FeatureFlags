using StronglyTyped.FeatureFlags;
using StronglyTyped.FeatureFlags.Abstractions;

namespace StronglyTyped.FeatureFlags.Consumer;

[FeatureFlagsHolder]
public static partial class FeatureFlags {
    private static readonly (string Feature, string Provider)[] _flags = {
        ("SaluteUniverse", "Configuration")
    };
}
