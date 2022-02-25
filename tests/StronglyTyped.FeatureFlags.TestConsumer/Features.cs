﻿namespace StronglyTyped.FeatureFlags.TestConsumer;

[FeatureList(nameof(_availableFeatures))]
public partial class Features {
    private static readonly string[] _availableFeatures = {
        "SaluteUniverse",
        "OtherFeature",
        nameof(SubFeatures)
    };
}
