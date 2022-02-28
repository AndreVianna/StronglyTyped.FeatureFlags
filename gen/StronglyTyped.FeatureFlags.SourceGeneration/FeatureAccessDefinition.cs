namespace StronglyTyped.FeatureFlags.SourceGeneration;

internal class FeatureAccessDefinition {
    public FeatureAccessDefinition(string @namespace, string name) {
        Namespace = @namespace;
        Name = name;
    }
    public string Namespace { get; }
    public string Name { get; }
    public ICollection<string> Features { get; } = new HashSet<string>();
    public ICollection<string> Sections { get; } = new HashSet<string>();
}
