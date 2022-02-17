namespace StronglyTyped.FeatureFlags.Abstractions;

public record Flag(bool IsEnabled) : IFlag {
    public static IFlag NullFlag => new Flag(false);
}
