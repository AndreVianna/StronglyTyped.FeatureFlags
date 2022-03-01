﻿namespace StronglyTyped.FeatureFlags.SourceGeneration;

internal class Emitter {
    private const int _defaultStringBuilderCapacity = 1024;
    private readonly StringBuilder _builder = new(_defaultStringBuilderCapacity);

    internal void EmitFiles(SourceProductionContext context, IReadOnlyList<FeaturesSectionDefinition> definitions) {
        foreach (var definition in definitions) {
            context.CancellationToken.ThrowIfCancellationRequested();
            var interfaceCode = EmitInterface(definition);
            context.AddSource($"I{definition.ClassName}.g.cs", SourceText.From(interfaceCode, Encoding.UTF8));

            var classCode = EmitClass(definition);
            context.AddSource($"{definition.ClassName}.g.cs", SourceText.From(classCode, Encoding.UTF8));
        }
    }

    private string EmitInterface(FeaturesSectionDefinition definition) {
        _builder.Clear();
        _builder.AppendLine("// <auto-generated/>");
        _builder.AppendLine("#nullable enable");
        GenInterface(definition);
        return _builder.ToString();
    }

    private string EmitClass(FeaturesSectionDefinition definition) {
        _builder.Clear();
        _builder.AppendLine("// <auto-generated/>");
        _builder.AppendLine("#nullable enable");
        GenClass(definition);
        return _builder.ToString();
    }

    private void GenInterface(FeaturesSectionDefinition definition) {
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


    private void GenClass(FeaturesSectionDefinition definition) {
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
        foreach (var feature in definition.Features)
            _builder.AppendLine($"    public IFeatureState {feature.Name} => _featureReader.For(nameof({feature.Name}));");

        if (definition.Sections.Any()) _builder.AppendLine();
        foreach (var section in definition.Sections)
            _builder.AppendLine($"    public I{section} {section} {{ get; }}");

        _builder.AppendLine("}");
    }
}
