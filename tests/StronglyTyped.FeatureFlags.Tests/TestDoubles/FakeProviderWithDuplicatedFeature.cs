namespace StronglyTyped.FeatureFlags.Tests.TestDoubles;

[ExcludeFromCodeCoverage]
internal class FakeProviderWithDuplicatedFeature : InMemoryProviderSpy {
    public FakeProviderWithDuplicatedFeature(ProcessSpy processSpy) : base(processSpy, new[] {
        new Feature("Feature2", FlagType.Transient, true),
    }) { }
}