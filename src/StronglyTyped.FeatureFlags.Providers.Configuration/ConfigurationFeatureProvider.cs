namespace StronglyTyped.FeatureFlags.Providers.Configuration;

public sealed class ConfigurationFeatureProvider : IFeatureProvider {
    private readonly IConfigurationSection _featuresSection;

    public ConfigurationFeatureProvider(IConfiguration config, string? featuresSectionName = null) {
        var sectionName = featuresSectionName ?? "Features";
        _featuresSection = config.GetSection(sectionName);
    }

    public string Name => "Configuration";

    public IEnumerable<IFeature> GetAll() {
        return _featuresSection.Exists()
            ? _featuresSection.GetChildren().Select(i => MapSection(i)!).ToArray()
            : Array.Empty<IFeature>();
    }

    public IFeature? GetByNameOrDefault(string featureName)
        => _featuresSection.Exists()
            ? MapSection(_featuresSection.GetSection(featureName))
            : null;

    public void Dispose() { }

    private static Feature? MapSection(IConfigurationSection section)
        => section.Exists()
            ? new Feature(section.Key, GetFlagType(section), GetFlagState(section))
            : null;

    private static FlagType GetFlagType(IConfiguration section) {
        return Enum.TryParse<FlagType>(section["Type"], true, out var enumValue)
                ? enumValue
                : FlagType.Static;
    }

    private static bool GetFlagState(IConfigurationSection section) {
        return bool.TryParse(section.Value, out var sectionValue) && sectionValue ||
               bool.TryParse(section["IsEnabled"], out var isEnabled) && isEnabled;
    }
}
