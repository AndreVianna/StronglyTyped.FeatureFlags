using System.Diagnostics.CodeAnalysis;

namespace StronglyTyped.FeatureFlags.TestConsumer;

[FeaturesSectionDefinition]
public partial class Features {
    [Features]
    [SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Used for code generation")]
    private static readonly string[] _features = {
        "SaluteUniverse",
        "OtherFeature",
    };
    [Features]
    [SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Used for code generation")]
    private static readonly string[] _moreFeatures = {
        "SaluteUniverse2",
        "OtherFeature2",
    };
    [Sections]
    [SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Used for code generation")]
    private static readonly string[] _sections = {
        nameof(SubFeatures)
    };
}
