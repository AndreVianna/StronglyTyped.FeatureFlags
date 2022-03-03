namespace StronglyTyped.FeatureFlags.Tests.TestDoubles;

[ExcludeFromCodeCoverage]
[FeaturesSectionDefinition]
public partial class TestSubFeatures {
    [Features]
    [SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Used for code generation")]
    private static readonly string[] _features = {
        "Feature3",
        "Feature4",
    };
}