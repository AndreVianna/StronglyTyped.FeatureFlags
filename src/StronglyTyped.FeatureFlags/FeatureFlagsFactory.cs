using System.Collections.Concurrent;

namespace StronglyTyped.FeatureFlags;
using static FlagType;

public sealed class FeatureFlagsFactory : IFlagsFactory {
    internal static readonly IDictionary<string, IFlag> StaticFlags = new Dictionary<string, IFlag>();
    internal static readonly ICollection<FeatureEntity> Features = new HashSet<FeatureEntity>();

    private readonly IServiceProvider _serviceProvider;
    private readonly IDictionary<string, IFlag> _scopedFlags = new Dictionary<string, IFlag>();

    public FeatureFlagsFactory(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
    }

    public IFlag For(string name) {
        if (StaticFlags.TryGetValue(name, out var flag)) return flag;
        if (_scopedFlags.ContainsKey(name)) return _scopedFlags[name];
        var feature = Features.FirstOrDefault(i => i.Name == name);
        if (feature is null) return new NullFlag();
        using var provider = (IFeatureProvider)_serviceProvider.GetRequiredService(feature.ProviderType);
        flag = (IFlag?)provider.GetByNameOrDefault(name) ?? new NullFlag();
        if (feature.FlagType == Scoped) _scopedFlags[name] = flag;
        return flag;
    }
}