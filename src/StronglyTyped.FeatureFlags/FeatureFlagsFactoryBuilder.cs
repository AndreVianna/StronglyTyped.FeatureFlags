namespace StronglyTyped.FeatureFlags;

public sealed class FeatureFlagsFactoryBuilder : IFlagsFactoryOptions {
    private readonly IServiceCollection _services;

    public FeatureFlagsFactoryBuilder(IServiceCollection services) {
        _services = services;
    }

    public void TryAddProvider<TProvider>(Func<IServiceProvider, TProvider>? createProvider = null) where TProvider : class, IFeatureProvider {
        if (FeatureFlagsFactory.Features.Any(i => i.ProviderType == typeof(TProvider))) return;
        if (createProvider is not null) _services.TryAddTransient(createProvider);
        else _services.TryAddTransient<TProvider>();
        RegisterProvider<TProvider>();
    }

    private void RegisterProvider<TProvider>() where TProvider : class, IFeatureProvider {
        using var serviceProvider = _services.BuildServiceProvider();
        RegisterFeatures(serviceProvider.GetRequiredService<TProvider>());
    }

    private static void RegisterFeatures<TProvider>(TProvider provider) where TProvider : class, IFeatureProvider {
        var features = provider.GetAll().ToArray();
        EnsureFeatureUniqueness<TProvider>(features);
        foreach (var feature in features) {
            FeatureFlagsFactory.Features.Add(new FeatureEntity(feature.Name, feature.Type, typeof(TProvider)));
            if (feature.Type == FlagType.Static) FeatureFlagsFactory.StaticFlags.Add(feature.Name, feature);
        }
    }

    private static void EnsureFeatureUniqueness<TProvider>(IEnumerable<IFeature> features) where TProvider : class, IFeatureProvider {
        var duplicatedFeatures = FeatureFlagsFactory.Features
            .Join(features, e => e.Name, f => f.Name, (i, _) => i)
            .GroupBy(i => i.ProviderType).ToArray();
        if (duplicatedFeatures.Length == 0) return;

        var message = new StringBuilder();
        message.Append($"Duplicated feature(s) found while registering the  '{typeof(TProvider).Name}' provider:\r\n");
        foreach (var group in duplicatedFeatures) {
            var featureList = string.Join(", ", group.Select(i => $"'{i.Name}'"));
            message.Append($"\t{featureList} found in '{group.Key.Name}' provider.\r\n");
        }
        throw new InvalidOperationException(message.ToString());
    }

    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Builder pattern.")]
    internal FeatureFlagsFactory Build(IServiceProvider serviceProvider) => new(serviceProvider);
}