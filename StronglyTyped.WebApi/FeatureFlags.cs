using StronglyTyped.FeatureFlags.Abstractions;

namespace StronglyTyped.WebApi;

[FeatureFlagsHolder]
public static partial class FeatureFlags {
    private static readonly ICollection<FeatureFlagDescriptor> _flags = new List<FeatureFlagDescriptor> {
        new FeatureFlagDescriptor("SaluteUniverse", FeatureFlagType.Static, "Configuration")
    };
}
