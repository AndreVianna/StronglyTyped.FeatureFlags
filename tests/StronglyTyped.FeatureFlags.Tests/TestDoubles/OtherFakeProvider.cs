namespace StronglyTyped.FeatureFlags.Tests.TestDoubles;

[ExcludeFromCodeCoverage]
internal class OtherFakeProvider : InMemoryProviderSpy {
    public OtherFakeProvider(ProcessSpy processSpy) : base(processSpy, new[] {
        new Feature("Feature4", FlagType.Static, true),
        new Feature("Feature5", FlagType.Scoped, true),
        new Feature("Feature6", FlagType.Transient, true),
    }) { }
}