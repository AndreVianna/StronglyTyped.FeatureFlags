namespace StronglyTyped.FeatureFlags;
using static FeatureFlagType;

public sealed class FeatureFlagsFactory : IFeatureFlagsFactory, IDisposable {

    private readonly IDictionary<string, IFeatureFlagProvider> _providers = new Dictionary<string, IFeatureFlagProvider>();
    private readonly IDictionary<string, string> _featureProviders = new Dictionary<string, string>();
    private readonly IDictionary<string, IFeatureFlag> _staticFlags = new Dictionary<string, IFeatureFlag>();

    public void AddProvider(IFeatureFlagProvider provider) {
        if (_providers.ContainsKey(provider.Name)) return;
        _providers[provider.Name] = provider;
        var flags = provider.GetAll().ToArray();
        foreach (var flag in flags) {
            if (_featureProviders.ContainsKey(flag.Name))
                throw new InvalidOperationException($"Duplicated feature flag definition found in '{_featureProviders[flag.Name]}' and '{provider.Name}' providers.");
            _featureProviders.Add(flag.Name, provider.Name);
            if (flag.Type == Static) _staticFlags.Add(flag.Name, flag);
        }
    }

    public IFeatureFlag For(string featureName) {
        return _staticFlags.TryGetValue(featureName, out var staticFlag)
            ? staticFlag
            : _featureProviders.TryGetValue(featureName, out var providerName)
                ? _providers[providerName].GetByName(featureName)
                : throw new InvalidOperationException("Feature flag not found.");
    }

    public void Dispose() {
        foreach (var providerName in _providers.Keys) {
            var provider = _providers[providerName];
            provider.Dispose();
        }
    }
}