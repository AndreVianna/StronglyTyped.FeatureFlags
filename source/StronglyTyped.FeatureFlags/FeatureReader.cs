using System.Collections.Concurrent;

namespace StronglyTyped.FeatureFlags;
using static FeatureStateLifecycle;

public sealed class FeatureReader : IFeatureReader {
    private readonly IServiceProvider _serviceProvider;
    private readonly IDictionary<string, IFeatureState> _cachedFlags;

    internal static readonly IDictionary<string, IFeatureState> StaticFlags = new Dictionary<string, IFeatureState>();
    internal static readonly ICollection<Feature> Features = new HashSet<Feature>();

    internal static string SerializePath(string[] path) => string.Join(":", path);

    public FeatureReader(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        _cachedFlags = new Dictionary<string, IFeatureState>();
    }

    public IFeatureState For(params string[] path) {
        var key = SerializePath(path);
        if (StaticFlags.TryGetValue(key, out var flag)) return flag;
        if (_cachedFlags.ContainsKey(key)) return _cachedFlags[key];
        var feature = Features.FirstOrDefault(i => i.Path.SequenceEqual(path));
        if (feature is null) return Flag.Null;
        using var provider = (IFeatureProvider)_serviceProvider.GetRequiredService(feature.ProviderType);
        flag = provider.GetFromPathOrDefault(path) as IFeatureState ?? Flag.Null;
        if (feature.Lifecycle == Scoped) _cachedFlags[key] = flag;
        return flag;
    }
}