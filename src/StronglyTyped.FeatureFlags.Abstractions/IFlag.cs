namespace StronglyTyped.FeatureFlags.Abstractions;

public interface IFlag {
    bool IsEnabled { get; }
}
