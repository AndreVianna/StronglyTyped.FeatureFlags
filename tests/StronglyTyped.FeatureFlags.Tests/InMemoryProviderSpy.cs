using System.Collections.Generic;
using System.Linq;

namespace StronglyTyped.FeatureFlags.Tests;

internal class InMemoryProviderSpy : IFeatureProvider {
    private readonly ProcessSpy _processSpy;
    private readonly IList<IFeature> _features = new List<IFeature>();


    public InMemoryProviderSpy(ProcessSpy processSpy, List<IFeature> features) {
        _processSpy = processSpy;
        _processSpy.RegisterCall("Constructor");
        features.ForEach(f => _features.Add(f));
    }

    public virtual string Name {
        get {
            _processSpy.RegisterCall("Name");
            return nameof(InMemoryProviderSpy);
        }
    }

    public void Dispose() {
        _processSpy.RegisterCall("Dispose");
    }
    public IEnumerable<IFeature> GetAll() {
        _processSpy.RegisterCall("GetAll");
        return _features;
    }

    public IFeature? GetByName(string featureName) {
        _processSpy.RegisterCall($"GetByName({featureName})");
        return featureName == "Feature3"
            ? null // Simulates removed transient feature
            : _features.FirstOrDefault(i => i.Name == featureName);
    }
}