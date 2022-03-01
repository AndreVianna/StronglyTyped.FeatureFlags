﻿namespace StronglyTyped.FeatureFlags.Tests.TestDoubles;

[ExcludeFromCodeCoverage]
internal class FakeProviderWithDuplicatedFeature : InMemoryProviderSpy {
    public FakeProviderWithDuplicatedFeature(ProcessSpy processSpy) : base(processSpy, new[] {
        new Feature(new [] { "Feature2" }, typeof(FakeProviderWithDuplicatedFeature), FeatureStateLifecycle.Transient, true),
    }) { }
}