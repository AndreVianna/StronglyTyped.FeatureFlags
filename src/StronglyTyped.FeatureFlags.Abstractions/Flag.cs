namespace StronglyTyped.FeatureFlags;

public record Flag(bool IsEnabled) : IFlag;