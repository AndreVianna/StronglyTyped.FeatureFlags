﻿namespace StronglyTyped.FeatureFlags.SourceGeneration;

internal class Emitter {
    private const int _defaultStringBuilderCapacity = 1024;
    private readonly StringBuilder _builder = new(_defaultStringBuilderCapacity);

    internal void EmitFiles(SourceProductionContext context, IReadOnlyList<FeatureAccessDefinition> definitions) {
        foreach (var definition in definitions) {
            context.CancellationToken.ThrowIfCancellationRequested();
            var interfaceCode = EmitInterface(definition);
            context.AddSource($"I{definition.ClassName}.g.cs", SourceText.From(interfaceCode, Encoding.UTF8));

            var classCode = EmitClass(definition);
            context.AddSource($"{definition.ClassName}.g.cs", SourceText.From(classCode, Encoding.UTF8));
        }
    }

    private string EmitInterface(FeatureAccessDefinition definition) {
        _builder.Clear();
        _builder.AppendLine("// <auto-generated/>");
        _builder.AppendLine("#nullable enable");
        GenInterface(definition);
        return _builder.ToString();
    }

    private string EmitClass(FeatureAccessDefinition definition) {
        _builder.Clear();
        _builder.AppendLine("// <auto-generated/>");
        _builder.AppendLine("#nullable enable");
        GenClass(definition);
        return _builder.ToString();
    }

    private void GenInterface(FeatureAccessDefinition definition) {
        var features = definition.Groups.SelectMany(i => i.Features.Select(f => new { i.Path, Name = f })).ToArray();
        var sections = definition.Groups.SelectMany(i => i.Sections.Select(s => new { i.Path, Name = s })).ToArray();

        _builder.Append(@$"
using StronglyTyped.FeatureFlags;

namespace {definition.Namespace};

public interface I{definition.ClassName}
{{
");
        foreach (var feature in features)
            _builder.AppendLine($"    IFeatureState {feature.Name} {{ get; }}");

        if (sections.Any() && features.Any()) _builder.AppendLine();

        foreach (var section in sections)
            _builder.AppendLine($"    I{section.Name} {section.Name} {{ get; }}");

        _builder.AppendLine("}");
    }


    private void GenClass(FeatureAccessDefinition definition) {
        var features = definition.Groups.SelectMany(i => i.Features.Select(f => new { i.Path, Name = f })).ToArray();
        var sections = definition.Groups.SelectMany(i => i.Sections.Select(s => new { i.Path, Name = s })).ToArray();

        _builder.Append(@$"
using StronglyTyped.FeatureFlags;

namespace {definition.Namespace};

partial class {definition.ClassName} : I{definition.ClassName}
{{
    private readonly IFeatureReader _featureReader;

    public {definition.ClassName}(IFeatureReader featureReader)
    {{
        _featureReader = featureReader;
");

        foreach (var section in sections)
            _builder.AppendLine($"        {section.Name} = new {section.Name}(_featureReader);");
        _builder.AppendLine("    }");

        if (features.Any()) _builder.AppendLine();
        foreach (var feature in features)
            _builder.AppendLine($"    public IFeatureState {feature.Name} => _featureReader.For(nameof({feature.Name}));");

        if (sections.Any()) _builder.AppendLine();
        foreach (var section in sections)
            _builder.AppendLine($"    public I{section.Name} {section.Name} {{ get; }}");

        _builder.AppendLine("}");
    }
}
