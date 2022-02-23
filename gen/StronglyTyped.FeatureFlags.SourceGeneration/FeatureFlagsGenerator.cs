namespace StronglyTyped.FeatureFlags.SourceGeneration;

[Generator]
public class FeatureFlagsGenerator : IIncrementalGenerator {

    public void Initialize(IncrementalGeneratorInitializationContext context) {
        var flagsSelectorClasses = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (sn, _) => Parser.IsSyntaxTargetForGeneration(sn),
                static (ctx, _) => Parser.GetSemanticTargetForGeneration(ctx))
            .Where(result => result is not null)
            .Collect();

        context.RegisterSourceOutput(flagsSelectorClasses, static (spc, source) => GenerateFiles(spc, source.Select(i => i!.Value).ToArray()));
    }

    internal static void GenerateFiles(SourceProductionContext context, (ClassDeclarationSyntax ClassDeclaration, string FieldName)[] selectors) {
        if (selectors.Length == 0)
            return;

        var parser = new Parser();
        var flagsSelectors = parser.GetFlagsSelectors(selectors, context.CancellationToken);

        var emitter = new Emitter();
        emitter.EmitFiles(context, flagsSelectors);
    }
}

