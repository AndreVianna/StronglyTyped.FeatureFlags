namespace StronglyTyped.FeatureFlags.SourceGeneration;

[Generator]
public class FeatureFlagsGenerator : IIncrementalGenerator {

    public void Initialize(IncrementalGeneratorInitializationContext context) {
        var flagsSelectorClasses = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (sn, _) => Parser.IsSyntaxTargetForGeneration(sn),
                static (ctx, _) => Parser.GetSemanticTargetForGeneration(ctx))
            .Where(type => type is not null)
            .Collect();

        context.RegisterSourceOutput(flagsSelectorClasses, static (spc, source) => GenerateFiles(spc, source!));
    }

    internal static void GenerateFiles(SourceProductionContext context, ImmutableArray<ClassDeclarationSyntax> classesWithFlagsSelectorAttribute) {
        if (classesWithFlagsSelectorAttribute.IsDefaultOrEmpty)
            return;

        var parser = new Parser();
        var flagsSelectors = parser.GetFlagsSelectors(classesWithFlagsSelectorAttribute, context.CancellationToken);

        var emitter = new Emitter();
        emitter.EmitFiles(context, flagsSelectors);
    }
}

