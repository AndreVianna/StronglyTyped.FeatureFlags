namespace StronglyTyped.FeatureFlags.Tests.TestDoubles;

[ExcludeFromCodeCoverage]
internal class FakeProvider : InMemoryProviderSpy {
    public FakeProvider(ProcessSpy processSpy) : base(processSpy, new[] {
        new Feature(new [] { "Feature1" }, typeof(FakeProvider), FeatureStateLifecycle.Static, true),
        new Feature(new [] { "Feature2" }, typeof(FakeProvider), FeatureStateLifecycle.Scoped, true),
        new Feature(new [] { "Feature3" }, typeof(FakeProvider), FeatureStateLifecycle.Transient, true),
        new Feature(new [] { "Feature7" }, typeof(FakeProvider), FeatureStateLifecycle.Scoped, true),
        new Feature(new [] { "Feature8" }, typeof(FakeProvider), FeatureStateLifecycle.Transient, true),
    }) { }
}