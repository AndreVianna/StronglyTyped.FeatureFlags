﻿// ReSharper disable once CheckNamespace
namespace StronglyTyped.FeatureFlags;

public interface IFlagsFactory {
    IFlag For(string featureName);
}