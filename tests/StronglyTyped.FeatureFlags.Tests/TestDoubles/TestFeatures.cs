namespace StronglyTyped.FeatureFlags.Tests.TestDoubles;

[ExcludeFromCodeCoverage]
[FeaturesSectionDefinition]
public partial class TestFeatures {
    [Features]
    [SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Used for code generation")]
    private static readonly string[] _features = {
        "Feature1",
        "Feature2",
    };
    [Sections]
    [SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Used for code generation")]
    private static readonly string[] _sections = {
        nameof(TestSubFeatures)
    };
}