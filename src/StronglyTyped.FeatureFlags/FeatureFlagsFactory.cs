namespace StronglyTyped.FeatureFlags;
using static FlagType;

public sealed class FeatureFlagsFactory : IFlagsFactory {
    internal static readonly IDictionary<string, IFlag> StaticFlags = new Dictionary<string, IFlag>();
    internal static readonly ICollection<FeatureEntity> Features = new HashSet<FeatureEntity>();

    private readonly IServiceProvider _serviceProvider;
    private readonly IDictionary<string, IFlag> _scopedFlags = new Dictionary<string, IFlag>();

    public FeatureFlagsFactory(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        RegisterScopedFeatures();
    }

    public IFlag For(string name) {
        if (StaticFlags.TryGetValue(name, out var staticFlag))
            return staticFlag;
        else if (_scopedFlags.TryGetValue(name, out var scopedFlag))
            return scopedFlag;
        else if (TryGetFromProvider(name, out var transientFlag))
            return transientFlag!;
        else
            return new NullFlag();
    }

    private void RegisterScopedFeatures() {
        var scopedFeaturesGroupedByProvider = Features
            .Where(i => i.FlagType == Scoped)
            .GroupBy(i => i.ProviderType)
            .Select(g => new { ProviderType = g.Key, FeatureNames = g.Select(i => i.Name) })
            .ToArray();
        foreach (var groupOfFeatures in scopedFeaturesGroupedByProvider) {
            using var provider = (IFeatureProvider)_serviceProvider.GetRequiredService(groupOfFeatures.ProviderType);
            foreach (var featureName in groupOfFeatures.FeatureNames)
                _scopedFlags[featureName] = provider.GetByNameOrDefault(featureName) ?? (IFlag)new NullFlag();
        }
    }

    private bool TryGetFromProvider(string featureName, out IFlag? flag) {
        flag = default;
        var feature = Features.FirstOrDefault(i => i.Name == featureName);
        if (feature is null) return false;

        using var provider = (IFeatureProvider)_serviceProvider.GetRequiredService(feature.ProviderType);
        flag = provider.GetByNameOrDefault(featureName);
        return flag is not null;
    }
}