namespace StronglyTyped.FeatureFlags;

public sealed class ConfigurationFeatureFlagsProvider : IFeatureFlagProvider {
    private readonly IConfiguration _config;

    public ConfigurationFeatureFlagsProvider(IConfiguration config) {
        _config = config;
    }

    public string Name => "Configuration";

    public IEnumerable<IFeatureFlag> GetAll() {
        return _config.GetSection("Features").GetChildren().Select(MapSection).ToArray();
    }

    public IFeatureFlag GetByName(string featureName) => throw new NotImplementedException();

    public void Dispose() { }

    private FeatureFlag MapSection(IConfigurationSection s)
        => new(s.Key,
            Enum.Parse<FeatureFlagType>(s["Type"] ?? "Static", true),
            bool.Parse(s["IsEnabled"] ?? "false"));
}
