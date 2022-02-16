namespace StronglyTyped.FeatureFlags.Generator;

internal class FlagsSelector {
    public FlagsSelector(string @namespace, string name) {
        Namespace = @namespace;
        Name = name;
    }
    public string Namespace { get; }
    public string Name { get; }
    public ICollection<string> Features { get; } = new HashSet<string>();
}
