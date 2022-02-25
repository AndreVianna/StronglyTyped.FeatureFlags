namespace StronglyTyped.FeatureFlags.Tests.TestDoubles;

[ExcludeFromCodeCoverage]
internal class FakeProvider : InMemoryProviderSpy {
    public FakeProvider(ProcessSpy processSpy) : base(processSpy, new[] {
        new Feature("Feature1", typeof(FakeProvider), FeatureStateLifecycle.Static, true),
        new Feature("Feature2", typeof(FakeProvider), FeatureStateLifecycle.Scoped, true),
        new Feature("Feature3", typeof(FakeProvider), FeatureStateLifecycle.Transient, true),
        new Feature("Feature7", typeof(FakeProvider), FeatureStateLifecycle.Scoped, true),
        new Feature("Feature8", typeof(FakeProvider), FeatureStateLifecycle.Transient, true),
    }) { }
}