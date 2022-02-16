namespace StronglyTyped.FeatureFlags.Abstractions;

public interface IFeatureFlag {
    bool IsEnabled { get; }
}
