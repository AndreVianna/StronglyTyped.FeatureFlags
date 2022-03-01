namespace StronglyTyped.FeatureFlags.SourceGeneration;

internal class FeatureGroup {
    public FeatureGroup(IEnumerable<string> path) {
        Path = path;
    }
    public IEnumerable<string> Path { get; }
    public ICollection<string> Features { get; } = new HashSet<string>();
    public ICollection<string> Sections { get; } = new HashSet<string>();
}

internal class FeatureAccessDefinition {
    public FeatureAccessDefinition(string @namespace, string className, IEnumerable<string> path) {
        Namespace = @namespace;
        ClassName = className;
        Path = path;
    }

    public string? Namespace { get; }
    public string ClassName { get; }
    public IEnumerable<string> Path { get; }
    public ICollection<FeatureGroup> Groups { get; } = new HashSet<FeatureGroup>();
}
