namespace StronglyTyped.FeatureFlags.Tests.TestDoubles;

[ExcludeFromCodeCoverage]
internal class FakeProvider : InMemoryProviderSpy {
    public FakeProvider(ProcessSpy processSpy) : base(processSpy, new[] {
        new Feature("Feature1", FlagType.Static, true),
        new Feature("Feature2", FlagType.Scoped, true),
        new Feature("Feature3", FlagType.Transient, true),
        new Feature("Feature7", FlagType.Scoped, true),
        new Feature("Feature8", FlagType.Transient, true),
    }) { }
}