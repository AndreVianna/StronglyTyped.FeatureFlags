// ReSharper disable once CheckNamespace
namespace StronglyTyped.FeatureFlags;

public record NullFlag : IFlag {
    public bool IsEnabled => false;
}