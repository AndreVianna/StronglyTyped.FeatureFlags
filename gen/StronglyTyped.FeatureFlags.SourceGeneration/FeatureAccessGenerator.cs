namespace StronglyTyped.FeatureFlags.SourceGeneration;

[Generator]
public class FeatureAccessGenerator : IIncrementalGenerator {

    public void Initialize(IncrementalGeneratorInitializationContext context) {
        var flagsSelectorClasses = context.SyntaxProvider
            .CreateSyntaxProvider(
                static (sn, _) => Parser.IsSyntaxTargetForGeneration(sn),
                static (ctx, _) => Parser.GetSemanticTargetForGeneration(ctx))
            .Where(result => result is not null)
            .Collect();

        context.RegisterSourceOutput(flagsSelectorClasses, static (spc, source) => GenerateFiles(spc, source!));
    }

    internal static void GenerateFiles(SourceProductionContext context, ImmutableArray<FeaturesSectionDefinition> selectors) {
        if (selectors.Length == 0)
            return;

        var emitter = new Emitter();
        emitter.EmitFiles(context, selectors);
    }
}

