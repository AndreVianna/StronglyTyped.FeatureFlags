namespace StronglyTyped.FeatureFlags.Tests.TestDoubles;

[ExcludeFromCodeCoverage]
internal class OtherFakeProvider : InMemoryProviderSpy {
    public OtherFakeProvider(ProcessSpy processSpy) : base(processSpy, new[] {
        new Feature("Feature4", typeof(OtherFakeProvider), FeatureStateLifecycle.Static, true),
        new Feature("Feature5", typeof(OtherFakeProvider), FeatureStateLifecycle.Scoped, true),
        new Feature("Feature6", typeof(OtherFakeProvider), FeatureStateLifecycle.Transient, true),
    }) { }
}