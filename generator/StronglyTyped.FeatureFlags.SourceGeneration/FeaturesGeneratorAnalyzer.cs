namespace StronglyTyped.FeatureFlags.SourceGeneration;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class FeaturesGeneratorAnalyzer : DiagnosticAnalyzer {
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => Analyzer.SupportedDiagnostics;

    public override void Initialize(AnalysisContext context) {
        context.EnableConcurrentExecution();
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.Analyze | GeneratedCodeAnalysisFlags.ReportDiagnostics);
        context.RegisterSyntaxNodeAction(Analyzer.AnalyzeNode, SyntaxKind.ClassDeclaration);
    }
}