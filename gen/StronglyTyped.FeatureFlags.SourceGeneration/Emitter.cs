﻿namespace StronglyTyped.FeatureFlags.SourceGeneration;

internal class Emitter {
    private const int _defaultStringBuilderCapacity = 1024;
    private readonly StringBuilder _builder = new(_defaultStringBuilderCapacity);

    internal void EmitFiles(SourceProductionContext context, IReadOnlyList<SectionDefinition> definitions) {
        foreach (var definition in definitions) {
            context.CancellationToken.ThrowIfCancellationRequested();
            var interfaceCode = EmitInterface(definition);
            context.AddSource($"I{definition.ClassName}.g.cs", SourceText.From(interfaceCode, Encoding.UTF8));

            var classCode = EmitClass(definition);
            context.AddSource($"{definition.ClassName}.g.cs", SourceText.From(classCode, Encoding.UTF8));
        }
    }

    private string EmitInterface(SectionDefinition definition) {
        _builder.Clear();
        _builder.AppendLine("// <auto-generated/>");
        _builder.AppendLine("#nullable enable");
        GenInterface(definition);
        return _builder.ToString();
    }

    private string EmitClass(SectionDefinition definition) {
        _builder.Clear();
        _builder.AppendLine("// <auto-generated/>");
        _builder.AppendLine("#nullable enable");
        GenClass(definition);
        return _builder.ToString();
    }

    private void GenInterface(SectionDefinition definition) {
        _builder.Append(@$"
using StronglyTyped.FeatureFlags;

namespace {definition.Namespace};

public interface I{definition.ClassName}
{{
");
        foreach (var feature in definition.Features)
            _builder.AppendLine($"    IFeatureState {feature.Name} {{ get; }}");

        if (definition.Sections.Any() && definition.Features.Any()) _builder.AppendLine();

        foreach (var section in definition.Sections)
            _builder.AppendLine($"    I{section} {section} {{ get; }}");

        _builder.AppendLine("}");
    }


    private void GenClass(SectionDefinition definition) {
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

        foreach (var section in definition.Sections)
            _builder.AppendLine($"        {section} = new {section}(_featureReader);");
        _builder.AppendLine("    }");

        if (definition.Features.Any()) _builder.AppendLine();
        foreach (var (path, name) in definition.Features) {
            var fullPath = string.Join(", ", definition.Path.Concat(path).Append(name).Select(i => $"\"{i}\""));
            _builder.AppendLine($"    public IFeatureState {name} => _featureReader.For({fullPath});");
        }

        if (definition.Sections.Any()) _builder.AppendLine();
        foreach (var section in definition.Sections)
            _builder.AppendLine($"    public I{section} {section} {{ get; }}");

        _builder.AppendLine("}");
    }
}
