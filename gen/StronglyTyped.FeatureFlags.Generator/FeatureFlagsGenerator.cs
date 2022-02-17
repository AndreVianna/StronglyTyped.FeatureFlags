namespace StronglyTyped.FeatureFlags.Generator;

[Generator]
public class FeatureFlagsGenerator : IIncrementalGenerator {

    public void Initialize(IncrementalGeneratorInitializationContext context) {
        var flagsSelectorClasses = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (sn, _) => Parser.IsSyntaxTargetForGeneration(sn),
                static (ctx, _) => Parser.GetSemanticTargetForGeneration(ctx))
            .Where(type => type is not null)
            .Collect();

        context.RegisterSourceOutput(flagsSelectorClasses, static (spc, source) => GenerateFiles( spc, source!));
    }

    private static void GenerateFiles(SourceProductionContext context, ImmutableArray<ClassDeclarationSyntax> flagsSelectorClasses) {
        if (flagsSelectorClasses.IsDefaultOrEmpty)
            return;

        var parser = new Parser();
        var flagsSelectors = parser.FindFlagsSelectorClasses(flagsSelectorClasses, context.CancellationToken);
        if (flagsSelectors.Count == 0) return;

        var emitter = new Emitter();
        emitter.EmitFiles(context, flagsSelectors);
    }
}

