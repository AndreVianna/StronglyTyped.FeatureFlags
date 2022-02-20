using Microsoft.Extensions.DependencyInjection.Extensions;

namespace StronglyTyped.FeatureFlags;
using static FlagType;

public sealed class FeatureFlagsFactory : IFlagsFactory, IFlagsFactoryOptions {

    private readonly IServiceCollection _services;
    private readonly IDictionary<string, Type> _providerTypeByName = new Dictionary<string, Type>();
    private readonly IDictionary<string, string> _providerNameByFeatureName = new Dictionary<string, string>();
    private readonly IDictionary<string, IFlag> _staticFlags = new Dictionary<string, IFlag>();

    public FeatureFlagsFactory(IServiceCollection services) {
        _services = services;
    }

    public void AddProvider<TProvider>() where TProvider : class, IFeatureProvider {
        _services.TryAddScoped<TProvider>();

        using var serviceProvider = _services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        var provider = scope.ServiceProvider.GetRequiredService<TProvider>();

        if (_providerTypeByName.ContainsKey(provider.Name)) return;

        _providerTypeByName[provider.Name] = typeof(TProvider);

        RegisterFeatures(provider);
    }

    public IFlag For(string name) {
        return _staticFlags.TryGetValue(name, out var staticFlag)
            ? staticFlag
            : _providerNameByFeatureName.TryGetValue(name, out var providerName)
                ? GetFromProvider(providerName, name)
                : new NullFlag();
    }

    private void RegisterFeatures(IFeatureProvider provider) {
        var features = provider.GetAll().ToArray();
        foreach (var feature in features) {
            if (_providerNameByFeatureName.ContainsKey(feature.Name))
                throw new InvalidOperationException($"Duplicated feature flag definition found in '{_providerNameByFeatureName[feature.Name]}' and '{provider.Name}' providers.");
            _providerNameByFeatureName.Add(feature.Name, provider.Name);
            if (feature.Type == Static) _staticFlags.Add(feature.Name, feature);
        }
    }

    private IFlag GetFromProvider(string providerName, string featureName) {
        var providerType = _providerTypeByName[providerName];
        using var serviceProvider = _services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        var provider = (IFeatureProvider)scope.ServiceProvider.GetRequiredService(providerType);
        return provider.GetByName(featureName) ?? (IFlag)new NullFlag();
    }
}