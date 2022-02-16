namespace StronglyTyped.FeatureFlags.Abstractions;

public interface IFlagsFactory {
    IFlag For(string featureName);
}