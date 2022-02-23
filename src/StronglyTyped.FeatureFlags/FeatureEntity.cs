namespace StronglyTyped.FeatureFlags;

internal record FeatureEntity(string Name, FlagType FlagType, Type ProviderType);