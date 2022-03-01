namespace StronglyTyped.FeatureFlags.Tests.TestDoubles;

[ExcludeFromCodeCoverage]
internal class InMemoryProviderSpy : IFeatureProvider {
    private readonly ProcessSpy _processSpy;
    private readonly IList<IFeature> _features = new List<IFeature>();


    public InMemoryProviderSpy(ProcessSpy processSpy, IEnumerable<IFeature> features) {
        _processSpy = processSpy;
        _processSpy.RegisterCall("Constructor");
        features.ToList().ForEach(f => _features.Add(f));
    }

    public void Dispose() {
        _processSpy.RegisterCall("Dispose");
    }
    public IEnumerable<IFeature> GetAll() {
        _processSpy.RegisterCall("GetAll");
        return _features;
    }

    public IFeature? GetFromPathOrDefault(params string[] path) {
        _processSpy.RegisterCall($"GetFromPathOrDefault({string.Join(", ", path)})");
        return path.SequenceEqual(new [] { "Feature7" }) || path.SequenceEqual(new[] { "Feature8" })
            ? null // Simulates removed transient feature
            : _features.FirstOrDefault(i => i.Path.SequenceEqual(path));
    }
}