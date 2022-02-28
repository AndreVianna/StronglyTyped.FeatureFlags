using System.Collections.Concurrent;

namespace StronglyTyped.FeatureFlags;
using static FeatureStateLifecycle;

public sealed class FeatureReader : IFeatureReader {
    private readonly IServiceProvider _serviceProvider;
    private readonly IDictionary<string, IFeatureState> _cachedFlags;

    internal static readonly IDictionary<string, IFeatureState> StaticFlags = new Dictionary<string, IFeatureState>();
    internal static readonly ICollection<Feature> Features = new HashSet<Feature>();

    public FeatureReader(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _cachedFlags = new Dictionary<string, IFeatureState>();
    }

    public IFeatureState For(string name) {
        if (StaticFlags.TryGetValue(name, out var flag)) return flag;
        if (_cachedFlags.ContainsKey(name)) return _cachedFlags[name];
        var feature = Features.FirstOrDefault(i => i.Name == name);
        if (feature is null) return Flag.Null;
        using var provider = (IFeatureProvider)_serviceProvider.GetRequiredService(feature.ProviderType);
        flag = (IFeatureState?)provider.GetByNameOrDefault(name) ?? Flag.Null;
        if (feature.Lifecycle == Scoped) _cachedFlags[name] = flag;
        return flag;
    }
}