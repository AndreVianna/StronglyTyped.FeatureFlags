namespace StronglyTyped.FeatureFlags.SourceGeneration;

internal class FlagsSelector {

    public FlagsSelector(string @namespace, string name) {
        Namespace = @namespace;
        Name = name;
    }

    public ICollection<string> Features { get; } = new HashSet<string>();
    public string Namespace { get; }
    public string Name { get; }
}
