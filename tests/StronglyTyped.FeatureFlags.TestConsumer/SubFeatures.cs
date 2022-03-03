﻿using System.Diagnostics.CodeAnalysis;

namespace StronglyTyped.FeatureFlags.TestConsumer;

[FeaturesSectionDefinition]
public partial class SubFeatures {
    [Features]
    [SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Used for code generation")]
    private static readonly string[] _features = {
        "Feature1",
        "Feature2",
    };
}
