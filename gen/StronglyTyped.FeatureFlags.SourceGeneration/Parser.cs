namespace StronglyTyped.FeatureFlags.SourceGeneration;


internal class Parser {
    private const string _flagsSelectorAttribute = "StronglyTyped.FeatureFlags.FeatureFlagsSelectorAttribute";

    internal static bool IsSyntaxTargetForGeneration(SyntaxNode node) =>
        node is ClassDeclarationSyntax { AttributeLists.Count: > 0 };

    internal static ClassDeclarationSyntax? GetSemanticTargetForGeneration(GeneratorSyntaxContext context) {
        var classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;

        foreach (var attributeListSyntax in classDeclarationSyntax.AttributeLists)
            foreach (var attributeSyntax in attributeListSyntax.Attributes) {
                var methodSymbol = context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol as IMethodSymbol;
                if (methodSymbol is not null &&
                    methodSymbol.ContainingType.ToDisplayString() == _flagsSelectorAttribute) {
                    return classDeclarationSyntax;
                }
            }

        return null;
    }

    public IReadOnlyList<FlagsSelector> GetFlagsSelectors(IEnumerable<ClassDeclarationSyntax> classes, CancellationToken cancellationToken) {
        var results = new List<FlagsSelector>();
        foreach (var classDeclaration in classes.Distinct().GroupBy(x => x.SyntaxTree).SelectMany(i => i)) {
            cancellationToken.ThrowIfCancellationRequested();
            var (@namespace, name) = ExtractClassDefinition(classDeclaration);
            if (@namespace is null) continue;
            var result = new FlagsSelector(@namespace, name);
            foreach (var member in classDeclaration.Members) {
                if (member is not FieldDeclarationSyntax field) continue;
                if (field.Declaration.Type is not ArrayTypeSyntax arrayTypeSyntax) continue;
                if (!IsString(arrayTypeSyntax.ElementType)) continue;
                if (field.Declaration.Variables.Count != 1) continue;
                var initializerSyntax = field.Declaration.Variables.First().Initializer;
                if (initializerSyntax is null) continue;

                var arrayItems = initializerSyntax.Value.ChildNodes();
                foreach (var arrayItem in arrayItems) {
                    if (arrayItem is not LiteralExpressionSyntax literalSyntax) continue;
                    var feature = literalSyntax.ChildTokens().First().ValueText;
                    result.Features.Add(feature);
                }
            }
            results.Add(result);
        }

        return results;
    }

    private static (string?, string) ExtractClassDefinition(ClassDeclarationSyntax classDeclaration) {
        var @namespace = classDeclaration.Parent switch {
            FileScopedNamespaceDeclarationSyntax namespaceSyntax => namespaceSyntax.Name.ToString(),
            NamespaceDeclarationSyntax namespaceSyntax => namespaceSyntax.Name.ToString(),
            _ => null
        };
        var name = classDeclaration.Identifier.ToString();
        return (@namespace, name);
    }

    private static bool IsString(TypeSyntax type) {
        return type switch {
            PredefinedTypeSyntax pfs when pfs.ToString() == "string" => true,
            IdentifierNameSyntax ins when ins.ToString() == "String" => true,
            QualifiedNameSyntax qns when qns.ToString() == "System.String" => true,
            _ => false,
        };
    }
}