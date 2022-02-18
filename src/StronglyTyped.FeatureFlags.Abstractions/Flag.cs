namespace StronglyTyped.FeatureFlags;

public record Flag(bool IsEnabled) : IFlag {
    public static IFlag NullFlag => new Flag(false);
}
