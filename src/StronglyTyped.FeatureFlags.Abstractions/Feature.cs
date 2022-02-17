namespace StronglyTyped.FeatureFlags.Abstractions;

public record Feature(string Name, FlagType Type, bool IsEnabled) : IFeature;