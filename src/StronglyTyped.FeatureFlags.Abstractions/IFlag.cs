// ReSharper disable once CheckNamespace
namespace StronglyTyped.FeatureFlags;

public interface IFlag {
    bool IsEnabled { get; }
}
