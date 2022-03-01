namespace StronglyTyped.FeatureFlags.Tests.TestDoubles;

[ExcludeFromCodeCoverage]
internal class OtherFakeProvider : InMemoryProviderSpy {
    public OtherFakeProvider(ProcessSpy processSpy) : base(processSpy, new[] {
        new Feature(new [] { "Feature4" }, typeof(OtherFakeProvider), FeatureStateLifecycle.Static, true),
        new Feature(new [] { "Feature5" }, typeof(OtherFakeProvider), FeatureStateLifecycle.Scoped, true),
        new Feature(new [] { "Feature6" }, typeof(OtherFakeProvider), FeatureStateLifecycle.Transient, true),
    }) { }
}