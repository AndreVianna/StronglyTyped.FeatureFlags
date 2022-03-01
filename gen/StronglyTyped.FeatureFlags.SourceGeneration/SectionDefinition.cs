namespace StronglyTyped.FeatureFlags.SourceGeneration;

internal class SectionDefinition {
    public SectionDefinition(string @namespace, string className, IEnumerable<string> path) {
        Namespace = @namespace;
        ClassName = className;
        Path = path;
    }

    public string? Namespace { get; }
    public string ClassName { get; }
    public IEnumerable<string> Path { get; }
    public ICollection<(IEnumerable<string> Path, string Name)> Features { get; } = new HashSet<(IEnumerable<string> Path, string Name)>();
    public ICollection<string> Sections { get; } = new HashSet<string>();
}
