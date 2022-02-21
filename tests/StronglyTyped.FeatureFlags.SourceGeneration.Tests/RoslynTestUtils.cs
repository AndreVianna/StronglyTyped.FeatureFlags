using System.Collections.Immutable;
using System.Reflection;

//using Microsoft.CodeAnalysis.CodeActions;
//using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
//using Microsoft.CodeAnalysis.Diagnostics;
//using Microsoft.CodeAnalysis.Text;

namespace StronglyTyped.FeatureFlags.SourceGeneration.Tests;

internal static class RoslynTestUtils {
    public static Project CreateTestProject(IEnumerable<Assembly> references, bool includeBaseReferences = true) {
        var coreLib = Assembly.GetAssembly(typeof(object))!.Location;
        var runtimeDir = Path.GetDirectoryName(coreLib)!;

        var metadataReferences = new List<MetadataReference>();
        if (includeBaseReferences) {
            metadataReferences.Add(MetadataReference.CreateFromFile(coreLib));
            metadataReferences.Add(MetadataReference.CreateFromFile(Path.Combine(runtimeDir, "netstandard.dll")));
            metadataReferences.Add(MetadataReference.CreateFromFile(Path.Combine(runtimeDir, "System.Runtime.dll")));
        }

        metadataReferences.AddRange(references.Select(r => MetadataReference.CreateFromFile(r.Location)));

        return new AdhocWorkspace()
            .AddSolution(SolutionInfo.Create(SolutionId.CreateNewId(), VersionStamp.Create()))
            .AddProject("SourceGeneration.Tests", "SourceGeneration.Tests.dll", "C#")
            .WithMetadataReferences(metadataReferences)
            .WithCompilationOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary).WithNullableContextOptions(NullableContextOptions.Enable));
    }

    //public static Task CommitChanges(this Project project, params string[] ignorables) {
    //    Assert.True(project.Solution.Workspace.TryApplyChanges(project.Solution));
    //    return AssertNoDiagnostic(project, ignorables);
    //}

    //public static async Task AssertNoDiagnostic(this Project project, params string[] ignorables) {
    //    foreach (var doc in project.Documents) {
    //        var sm = await doc.GetSemanticModelAsync(CancellationToken.None).ConfigureAwait(false);
    //        Assert.NotNull(sm);

    //        foreach (var d in sm!.GetDiagnostics()) {
    //            var ignore = ignorables.Any(ig => d.Id == ig);

    //            Assert.True(ignore, d.ToString());
    //        }
    //    }
    //}

    private static Project WithDocuments(this Project project, IEnumerable<string> sources, IEnumerable<string>? sourceNames = null) {
        var count = 0;
        var result = project;
        if (sourceNames != null) {
            var names = sourceNames.ToList();
            result = sources.Aggregate(result, (current, s) => current.WithDocument(names[count++], s));
        }
        else {
            result = sources.Aggregate(result, (current, s) => current.WithDocument($"src-{count++}.cs", s));
        }

        return result;
    }

    public static Project WithDocument(this Project project, string name, string text) {
        return project.AddDocument(name, text).Project;
    }

    //public static Document FindDocument(this Project project, string name) {
    //    foreach (var doc in project.Documents) {
    //        if (doc.Name == name) {
    //            return doc;
    //        }
    //    }

    //    throw new FileNotFoundException(name);
    //}

    //public static TextSpan MakeSpan(string text, int spanNum) {
    //    var start = text.IndexOf($"/*{spanNum}+*/", StringComparison.Ordinal);
    //    if (start < 0) {
    //        throw new ArgumentOutOfRangeException(nameof(spanNum));
    //    }

    //    start += 6;

    //    var end = text.IndexOf($"/*-{spanNum}*/", StringComparison.Ordinal);
    //    if (end < 0) {
    //        throw new ArgumentOutOfRangeException(nameof(spanNum));
    //    }

    //    end -= 1;

    //    return new TextSpan(start, end - start);
    //}

    public static async Task<(ImmutableArray<Diagnostic>, ImmutableArray<GeneratedSourceResult>)> RunGeneratorAsync(
        string code,
        bool includeBaseReferences = true,
        bool includeReferences = true,
        CancellationToken cancellationToken = default) {

        var references = Array.Empty<Assembly>();
        if (includeReferences) {
            references = new[] { typeof(FeatureFlagsSelectorAttribute).Assembly };
        }

        var (diagnostics, generatedCode) = await RunGeneratorAsync(
            new FeatureFlagsGenerator(),
            references,
            new[] { code },
            includeBaseReferences,
            cancellationToken).ConfigureAwait(false);

        return (diagnostics, generatedCode);
    }

    public static async Task<(ImmutableArray<Diagnostic>, ImmutableArray<GeneratedSourceResult>)> RunGeneratorAsync(
        IIncrementalGenerator generator,
        IEnumerable<Assembly> references,
        IEnumerable<string> sources,
        bool includeBaseReferences = true,
        CancellationToken cancellationToken = default) {
        var project = CreateTestProject(references, includeBaseReferences).WithDocuments(sources);
        if (!project.Solution.Workspace.TryApplyChanges(project.Solution)) throw new InvalidOperationException("Failed to create project.");
        var compilation = await project.GetCompilationAsync(CancellationToken.None).ConfigureAwait(false);
        return RunGeneratorAsync(compilation!, generator, cancellationToken);
    }

    public static (ImmutableArray<Diagnostic>, ImmutableArray<GeneratedSourceResult>) RunGeneratorAsync(
        Compilation compilation,
        IIncrementalGenerator generator,
        CancellationToken cancellationToken = default) {
        var options = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Preview);
        var driver = CSharpGeneratorDriver.Create(new[] { generator.AsSourceGenerator() }, parseOptions: options);
        var generatorDriver = driver.RunGenerators(compilation, cancellationToken);
        var result = generatorDriver.GetRunResult();
        return (result.Results[0].Diagnostics, result.Results[0].GeneratedSources);
    }

    //public static async Task<IList<Diagnostic>> RunAnalyzer(
    //    DiagnosticAnalyzer analyzer,
    //    IEnumerable<Assembly> references,
    //    IEnumerable<string> sources) {
    //    var project = CreateTestProject(references);

    //    project = project.WithDocuments(sources);

    //    await project.CommitChanges().ConfigureAwait(false);

    //    var analyzers = ImmutableArray.Create(analyzer);

    //    var comp = await project!.GetCompilationAsync().ConfigureAwait(false);
    //    return await comp!.WithAnalyzers(analyzers).GetAllDiagnosticsAsync().ConfigureAwait(false);
    //}

    //public static async Task<IList<string>> RunAnalyzerAndFixer(
    //    DiagnosticAnalyzer analyzer,
    //    CodeFixProvider fixer,
    //    IEnumerable<Assembly> references,
    //    IEnumerable<string> sources,
    //    IEnumerable<string>? sourceNames = null,
    //    string? defaultNamespace = null,
    //    string? extraFile = null) {
    //    var project = CreateTestProject(references);

    //    var count = sources.Count();
    //    project = project.WithDocuments(sources, sourceNames);

    //    if (defaultNamespace != null) {
    //        project = project.WithDefaultNamespace(defaultNamespace);
    //    }

    //    await project.CommitChanges().ConfigureAwait(false);

    //    var analyzers = ImmutableArray.Create(analyzer);

    //    while (true) {
    //        var comp = await project!.GetCompilationAsync().ConfigureAwait(false);
    //        var diags = await comp!.WithAnalyzers(analyzers).GetAllDiagnosticsAsync().ConfigureAwait(false);
    //        if (diags.IsEmpty) {
    //            // no more diagnostics reported by the analyzers
    //            break;
    //        }

    //        var actions = new List<CodeAction>();
    //        foreach (var d in diags) {
    //            var doc = project.GetDocument(d.Location.SourceTree);

    //            var context = new CodeFixContext(doc!, d, (action, _) => actions.Add(action), CancellationToken.None);
    //            await fixer.RegisterCodeFixesAsync(context).ConfigureAwait(false);
    //        }

    //        if (actions.Count == 0) {
    //            // nothing to fix
    //            break;
    //        }

    //        var operations = await actions[0].GetOperationsAsync(CancellationToken.None).ConfigureAwait(false);
    //        var solution = operations.OfType<ApplyChangesOperation>().Single().ChangedSolution;
    //        var changedProj = solution.GetProject(project.Id);
    //        if (changedProj != project) {
    //            project = await RecreateProjectDocumentsAsync(changedProj!).ConfigureAwait(false);
    //        }
    //    }

    //    var results = new List<string>();

    //    if (sourceNames != null) {
    //        var l = sourceNames.ToList();
    //        for (var i = 0; i < count; i++) {
    //            var s = await project.FindDocument(l[i]).GetTextAsync().ConfigureAwait(false);
    //            results.Add(s.ToString().Replace("\r\n", "\n", StringComparison.Ordinal));
    //        }
    //    }
    //    else {
    //        for (var i = 0; i < count; i++) {
    //            var s = await project.FindDocument($"src-{i}.cs").GetTextAsync().ConfigureAwait(false);
    //            results.Add(s.ToString().Replace("\r\n", "\n", StringComparison.Ordinal));
    //        }
    //    }

    //    if (extraFile != null) {
    //        var s = await project.FindDocument(extraFile).GetTextAsync().ConfigureAwait(false);
    //        results.Add(s.ToString().Replace("\r\n", "\n", StringComparison.Ordinal));
    //    }

    //    return results;
    //}

    //private static async Task<Project> RecreateProjectDocumentsAsync(Project project) {
    //    foreach (var documentId in project.DocumentIds) {
    //        var document = project.GetDocument(documentId);
    //        document = await RecreateDocumentAsync(document!).ConfigureAwait(false);
    //        project = document.Project;
    //    }

    //    return project;
    //}

    //private static async Task<Document> RecreateDocumentAsync(Document document) {
    //    var newText = await document.GetTextAsync().ConfigureAwait(false);
    //    return document.WithText(SourceText.From(newText.ToString(), newText.Encoding, newText.ChecksumAlgorithm));
    //}
}
