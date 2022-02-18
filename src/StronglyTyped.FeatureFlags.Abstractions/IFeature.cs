namespace StronglyTyped.FeatureFlags;

public interface IFeature : IFlag {
    string Name { get; }
    FlagType Type { get; }
}