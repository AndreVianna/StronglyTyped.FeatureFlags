namespace StronglyTyped.FeatureFlags.Providers.Configuration;

public sealed class ConfigurationFeatureProvider : IFeatureProvider {
    private readonly string _basePath;
    private readonly IConfigurationSection _featuresSection;

    public ConfigurationFeatureProvider(IConfiguration config, params string[] basePath) {
        _basePath = basePath.Any() ? string.Join(":", basePath) : "Features";
        _featuresSection = config.GetSection(_basePath);
    }

    public IEnumerable<IFeature> GetAll() {
        if (!_featuresSection.Exists())
            return Array.Empty<IFeature>();

        var result = new List<IFeature>();
        AddDescendants(_featuresSection, result);
        return result.ToArray();
    }

    public IFeature? GetFromPathOrDefault(params string[] path) {
        if (!_featuresSection.Exists())
            return null;

        var key = string.Join(":", path);
        var child = _featuresSection.GetSection(key);
        return TryGetFeature(child, out var feature) ? feature : null;
    }

    public void Dispose() { }

    private void AddDescendants(IConfigurationSection section, ICollection<IFeature> features) {
        if (!section.Exists()) return;
        if (TryGetFeature(section, out var feature)) {
            features.Add(feature);
            return;
        }
        var children = section.GetChildren();
        foreach (var child in children) {
            AddDescendants(child, features);
        }
    }

    private bool TryGetFeature(IConfigurationSection section, out Feature feature) {
        feature = default!;
        if (!section.Exists()) return false;
        if (bool.TryParse(section.Value, out var sectionValue)) {
            var featurePath = GetFeaturePath(section);
            feature = new Feature(featurePath, typeof(ConfigurationFeatureProvider), FeatureStateLifecycle.Static, sectionValue);
            return true;
        }
        if (bool.TryParse(section["IsEnabled"], out var isEnabled)) {
            var lifecycle = GetFlagType(section);
            var featurePath = GetFeaturePath(section);
            feature = new Feature(featurePath, typeof(ConfigurationFeatureProvider), lifecycle, isEnabled);
            return true;
        }
        return false;
    }

    private string[] GetFeaturePath(IConfigurationSection section) {
        var featurePath = section.Path.StartsWith(_basePath) ? section.Path.Remove(0, _basePath.Length + 1) : section.Path;
        return featurePath.Split(":");
    }

    private static FeatureStateLifecycle GetFlagType(IConfiguration section) {
        return Enum.TryParse<FeatureStateLifecycle>(section["Type"], true, out var enumValue)
                ? enumValue
                : FeatureStateLifecycle.Static;
    }
}
