namespace StronglyTyped.FeatureFlags.Tests.TestDoubles;

[ExcludeFromCodeCoverage]
[FeaturesSectionDefinition]
public partial class TestFeaturesWithError {
    [Features]
    [SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Used for code generation")]
    private static readonly int[] _features = {
        1,
        2,
    };
}