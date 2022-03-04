namespace StronglyTyped.FeatureFlags.Tests.TestDoubles;

[ExcludeFromCodeCoverage]
[FeaturesSectionDefinition]
public partial class TestFeaturesWithError {
    [Features]
    private static readonly int[] _features = {
        1,
        2
    };
}