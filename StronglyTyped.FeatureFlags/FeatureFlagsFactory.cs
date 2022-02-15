namespace StronglyTyped.FeatureFlags;
using static FeatureFlagType;

public sealed class FeatureFlagsFactory : IFeatureFlagsFactory, IFeatureFlagsOptions, IDisposable {

    private readonly IServiceCollection _services;
    private readonly IDictionary<string, IFeatureFlagProvider> _providers = new Dictionary<string, IFeatureFlagProvider>();
    private readonly IDictionary<string, string> _featureProviders = new Dictionary<string, string>();
    private readonly IDictionary<string, IFeatureFlag> _staticFlags = new Dictionary<string, IFeatureFlag>();

    public FeatureFlagsFactory(IServiceCollection services) {
        _services = services;
    }

    public void AddProvider<TProvider>() where TProvider : class, IFeatureFlagProvider {
        _services.AddSingleton<TProvider>();
        var serviceProvider = _services.BuildServiceProvider();
        var flagsProvider = serviceProvider.GetRequiredService<TProvider>();
        if (_providers.ContainsKey(flagsProvider.Name)) return;
        _providers[flagsProvider.Name] = flagsProvider;
        var flags = flagsProvider.GetAll().ToArray();
        foreach (var flag in flags) {
            if (_featureProviders.ContainsKey(flag.Name))
                throw new InvalidOperationException($"Duplicated feature flag definition found in '{_featureProviders[flag.Name]}' and '{flagsProvider.Name}' providers.");
            _featureProviders.Add(flag.Name, flagsProvider.Name);
            if (flag.Type == Static) _staticFlags.Add(flag.Name, flag);
        }
    }

    public IFeatureFlag For(string name) {
        return _staticFlags.TryGetValue(name, out var staticFlag)
            ? staticFlag
            : _featureProviders.TryGetValue(name, out var providerName)
                ? _providers[providerName].GetByName(name)
                : FeatureFlag.GetNullFlagFor(name);
    }

    public void Dispose() {
        foreach (var providerName in _providers.Keys) {
            var provider = _providers[providerName];
            provider.Dispose();
        }
    }
}