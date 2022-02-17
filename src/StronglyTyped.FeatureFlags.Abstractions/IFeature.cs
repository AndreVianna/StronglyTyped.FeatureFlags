namespace StronglyTyped.FeatureFlags.Abstractions;

public interface IFeature : IFlag {
    string Name { get; }
    FlagType Type { get; }
}