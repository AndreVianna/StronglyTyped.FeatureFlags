﻿// ReSharper disable once CheckNamespace
namespace StronglyTyped.FeatureFlags;

public interface IFeatureReader {
    IFeatureState For(params string[] path);
}