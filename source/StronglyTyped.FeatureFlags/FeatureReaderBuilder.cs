namespace StronglyTyped.FeatureFlags;

public sealed class FeatureReaderBuilder : IFeatureReaderBuilderOptions {
    private readonly IServiceCollection _services;

    public FeatureReaderBuilder(IServiceCollection services) {
        _services = services;
    }

    public void TryAddProvider<TProvider>(Func<IServiceProvider, TProvider>? createProvider = null) where TProvider : class, IFeatureProvider {
        if (FeatureReader.Features.Any(i => i.ProviderType == typeof(TProvider))) return;
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
            FeatureReader.Features.Add(new Feature(feature.Path, typeof(TProvider), feature.Lifecycle, feature.IsEnabled));
            if (feature.Lifecycle == FeatureStateLifecycle.Static) FeatureReader.StaticFlags.Add(FeatureReader.SerializePath(feature.Path), feature);
        }
    }

    private class FeatureComparer : IEqualityComparer<string[]> {
        private FeatureComparer() { }
        internal static FeatureComparer Default => new();
        public bool Equals(string[]? x, string[]? y) => x!.SequenceEqual(y!);
        public int GetHashCode(string[] obj) => obj.Aggregate(0, HashCode.Combine);
    }

    private static void EnsureFeatureUniqueness<TProvider>(IEnumerable<IFeature> features) where TProvider : class, IFeatureProvider {
        var duplicatedFeatures = FeatureReader.Features
            .Join(features, e => e.Path, f => f.Path, (i, _) => i, FeatureComparer.Default)
            .GroupBy(i => i.ProviderType).ToArray();
        if (duplicatedFeatures.Length == 0) return;

        var message = new StringBuilder();
        message.Append($"Duplicated feature(s) found while registering the  '{typeof(TProvider).Name}' provider:\r\n");
        foreach (var group in duplicatedFeatures) {
            var featureList = string.Join(", ", group.Select(i => $"'{string.Join(".", i.Path)}'"));
            message.Append($"\t{featureList} found in '{group.Key.Name}' provider.\r\n");
        }
        throw new InvalidOperationException(message.ToString());
    }

    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Builder pattern.")]
    internal FeatureReader Build(IServiceProvider serviceProvider) => new(serviceProvider);
}