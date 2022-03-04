using System.Reflection;

namespace StronglyTyped.FeatureFlags.SourceGeneration.Tests.Helpers {
    [ExcludeFromCodeCoverage]
    internal static class AnalyzerRunner {
        public static DiagnosticResult CreateDiagnosticResult(string diagnosticId) {
            var descriptor = Analyzer.SupportedDiagnostics.First(i => i.Id == diagnosticId);
            return CSharpAnalyzerVerifier<FeaturesGeneratorAnalyzer, XUnitVerifier>.Diagnostic(descriptor);
        }

        public static async Task VerifyCodeAsync(string code, params DiagnosticResult[] expected) {
            var test = new Test(code, expected);
        await test.RunAsync(CancellationToken.None);
        }

        private class Test : CSharpAnalyzerTest<FeaturesGeneratorAnalyzer, XUnitVerifier> {
            public Test(string code, params DiagnosticResult[] expected) {
                TestState.AdditionalReferences.AddRange(new [] {
                    MetadataReference.CreateFromFile(typeof(FeaturesSectionDefinitionAttribute).Assembly.Location)
                });
                TestCode = code;
                ExpectedDiagnostics.AddRange(expected);
                SolutionTransforms.Add((solution, projectId) => {
                    var compilationOptions = solution.GetProject(projectId)!.CompilationOptions!;
                    compilationOptions = compilationOptions.WithSpecificDiagnosticOptions(compilationOptions.SpecificDiagnosticOptions.SetItems(_nullableWarnings));
                    solution = solution.WithProjectCompilationOptions(projectId, compilationOptions);

                    return solution;
                });
                ReferenceAssemblies = new ReferenceAssemblies("net6.0", new PackageIdentity("Microsoft.NETCore.App.Ref", "6.0.0"), Path.Combine("ref", "net6.0"));
            }
        }

        private static readonly ImmutableDictionary<string, ReportDiagnostic> _nullableWarnings = GetNullableWarningsFromCompiler();

        private static ImmutableDictionary<string, ReportDiagnostic> GetNullableWarningsFromCompiler() {
            var args = new[] { "/warnaserror:nullable" };
            return CSharpCommandLineParser.Default
                .Parse(args, Environment.CurrentDirectory, Environment.CurrentDirectory)
                .CompilationOptions
                .SpecificDiagnosticOptions
                .SetItem("CS8632", ReportDiagnostic.Error)
                .SetItem("CS8669", ReportDiagnostic.Error);
        }
    }
}

