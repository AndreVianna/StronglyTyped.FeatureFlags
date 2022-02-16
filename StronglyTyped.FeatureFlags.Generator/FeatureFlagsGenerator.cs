namespace StronglyTyped.FeatureFlags.Generator;

[Generator]
public class FeatureFlagsGenerator : IIncrementalGenerator {

    public void Initialize(IncrementalGeneratorInitializationContext context) {
        var flagsHolderClasses = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (sn, _) => Parser.IsSyntaxTargetForGeneration(sn),
                static (ctx, _) => Parser.GetSemanticTargetForGeneration(ctx))
            .Where(type => type is not null)
            .Collect();

        context.RegisterSourceOutput(flagsHolderClasses, static (spc, source) => GenerateFiles( spc, source!));
    }

    private static void GenerateFiles(SourceProductionContext context, ImmutableArray<ClassDeclarationSyntax> flagsHolderClasses) {
        if (flagsHolderClasses.IsDefaultOrEmpty)
            return;

        var parser = new Parser();
        var flagsHolders = parser.GetFlagsHolderClasses(flagsHolderClasses, context.CancellationToken);
        if (flagsHolders.Count == 0) return;

        var emitter = new Emitter();
        emitter.EmitFiles(context, flagsHolders);
    }
}

