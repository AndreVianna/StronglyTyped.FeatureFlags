using System.Collections.Generic;

namespace StronglyTyped.FeatureFlags.Tests;

internal class FakeProvider : InMemoryProviderSpy {
    public FakeProvider(ProcessSpy processSpy) : base(processSpy, new List<IFeature> {
        new Feature("Feature1", FlagType.Static, true),
        new Feature("Feature2", FlagType.Transient, true),
        new Feature("Feature3", FlagType.Transient, true),
    }) { }

    public override string Name {
        get {
            _ = base.Name;
            return nameof(FakeProvider);
        }
    }
}