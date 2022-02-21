namespace StronglyTyped.FeatureFlags.SourceGeneration;

internal record FlagsSelector(string Namespace, string Name) {
    public ICollection<string> Features { get; } = new HashSet<string>();
}
