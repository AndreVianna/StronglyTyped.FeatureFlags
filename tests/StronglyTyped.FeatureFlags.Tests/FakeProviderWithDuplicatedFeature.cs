using System.Collections.Generic;

namespace StronglyTyped.FeatureFlags.Tests;

internal class FakeProviderWithDuplicatedFeature : InMemoryProviderSpy {
    public FakeProviderWithDuplicatedFeature(ProcessSpy processSpy) : base(processSpy, new List<IFeature> {
        new Feature("Feature2", FlagType.Transient, true),
    }) { }

    public override string Name {
        get {
            _ = base.Name;
            return nameof(FakeProviderWithDuplicatedFeature);
        }
    }
}