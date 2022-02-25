using System.Reflection;

using Microsoft.CodeAnalysis.CSharp;

namespace StronglyTyped.FeatureFlags.SourceGeneration.Tests.Helpers;

[ExcludeFromCodeCoverage]
internal static class GeneratorRunner {
    internal static async Task<(Diagnostic[], GeneratedSourceResult[])> RunAsync(string code) {
        var runResult = await RunGeneratorAsync(
            new FeatureAccessorGenerator(),
            new[] { typeof(FeatureListAttribute).Assembly },
            new[] { code }).ConfigureAwait(false);

        return (runResult.Diagnostics.ToArray(), runResult.GeneratedSources.ToArray());
    }

    private static async Task<GeneratorRunResult> RunGeneratorAsync(IIncrementalGenerator generator, IEnumerable<Assembly> references, IEnumerable<string> sources) {
        var project = CreateTestProject(references).WithDocuments(sources);
        if (!project.Solution.Workspace.TryApplyChanges(project.Solution))
            throw new InvalidOperationException("Failed to create project.");
        var compilation = await project.GetCompilationAsync().ConfigureAwait(false);
        return RunGenerator(compilation!, generator);
    }

    private static Project CreateTestProject(IEnumerable<Assembly> references) {
        var coreLib = Assembly.GetAssembly(typeof(object))!.Location;
        var runtimeDir = Path.GetDirectoryName(coreLib)!;

        var metadataReferences = new List<MetadataReference> {
            MetadataReference.CreateFromFile(coreLib),
            MetadataReference.CreateFromFile(Path.Combine(runtimeDir, "netstandard.dll")),
            MetadataReference.CreateFromFile(Path.Combine(runtimeDir, "System.Runtime.dll"))
        };
        metadataReferences.AddRange(references.Select(assembly => MetadataReference.CreateFromFile(assembly.Location)));

        return new AdhocWorkspace()
            .AddSolution(SolutionInfo.Create(SolutionId.CreateNewId(), VersionStamp.Create()))
            .AddProject("SourceGeneration.Tests", "SourceGeneration.Tests.dll", "C#")
            .WithMetadataReferences(metadataReferences)
            .WithCompilationOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary).WithNullableContextOptions(NullableContextOptions.Enable));
    }

    private static Project WithDocuments(this Project project, IEnumerable<string> sources)
        => sources
            .Select((source, index) => (source, index))
            .Aggregate(project, (current, file) => current.WithDocument($"src-{file.index}.cs", file.source));

    public static Project WithDocument(this Project project, string name, string source)
        => project.AddDocument(name, source).Project;

    private static GeneratorRunResult RunGenerator(Compilation compilation, IIncrementalGenerator generator) {
        var options = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Preview);
        var driver = CSharpGeneratorDriver.Create(new[] { generator.AsSourceGenerator() }, parseOptions: options);
        var generatorDriver = driver.RunGenerators(compilation);
        var result = generatorDriver.GetRunResult();
        return result.Results[0];
    }
}
