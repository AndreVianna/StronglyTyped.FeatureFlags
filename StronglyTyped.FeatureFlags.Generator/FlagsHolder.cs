namespace StronglyTyped.FeatureFlags.Generator;

internal class FlagsHolder {
    public FlagsHolder(string @namespace, string name) {
        Namespace = @namespace;
        Name = name;
    }
    public string Namespace { get; }
    public string Name { get; }
    public ICollection<string> Featrues { get; } = new HashSet<string>();
}
