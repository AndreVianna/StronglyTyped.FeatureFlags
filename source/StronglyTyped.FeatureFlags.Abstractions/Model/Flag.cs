// ReSharper disable once CheckNamespace
namespace StronglyTyped.FeatureFlags;

public record Flag(bool IsEnabled) : IFeatureState {
    public static IFeatureState Null => new Flag(false);
}