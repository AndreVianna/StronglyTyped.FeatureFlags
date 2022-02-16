namespace StronglyTyped.FeatureFlags;

public sealed class ConfigurationFeatureProvider : IFeatureProvider {
    private readonly IConfiguration _config;

    public ConfigurationFeatureProvider(IConfiguration config) {
        _config = config;
    }

    public string Name => "Configuration";

    public IEnumerable<IFeature> GetAll() {
        return _config.GetSection("Features").GetChildren().Select(MapSection).ToArray();
    }

    public IFeature GetByName(string featureName) => throw new NotImplementedException();

    public void Dispose() { }

    private Feature MapSection(IConfigurationSection s)
        => new(s.Key,
            Enum.TryParse<FlagType>(s["Type"], true, out var type) ? type : FlagType.Static,
            (bool.TryParse(s.Value, out var isEnabled) && isEnabled) || (bool.TryParse(s["IsEnabled"] ?? "false", out isEnabled) && isEnabled));
}
