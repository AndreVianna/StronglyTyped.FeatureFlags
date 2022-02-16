namespace StronglyTyped.FeatureFlags.Generator;


internal class Parser {
    private const string _flagsHolderAttribute = "StronglyTyped.FeatureFlags.FeatureFlagsHolderAttribute";

    internal static bool IsSyntaxTargetForGeneration(SyntaxNode node) =>
        node is ClassDeclarationSyntax { AttributeLists.Count: > 0 };

    internal static ClassDeclarationSyntax? GetSemanticTargetForGeneration(GeneratorSyntaxContext context) {
        var classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;

        foreach (var attributeListSyntax in classDeclarationSyntax.AttributeLists)
            foreach (var attributeSyntax in attributeListSyntax.Attributes) {
                var methodSymbol = context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol as IMethodSymbol;
                if (methodSymbol is not null &&
                    methodSymbol.ContainingType.ToDisplayString() == _flagsHolderAttribute) {
                    return classDeclarationSyntax;
                }
            }

        return null;
    }

    public IReadOnlyList<FlagsHolder> GetFlagsHolderClasses(IEnumerable<ClassDeclarationSyntax> classes, CancellationToken cancellationToken) {
        var results = new List<FlagsHolder>();
        foreach (var classDeclaration in classes.Distinct().GroupBy(x => x.SyntaxTree).SelectMany(i => i)) {
            cancellationToken.ThrowIfCancellationRequested();
            var (@namespace, name) = ExtractClassDefinition(classDeclaration);
            if (@namespace is null) continue;
            var result = new FlagsHolder(@namespace, name);
            foreach (var member in classDeclaration.Members) {
                if (member is not FieldDeclarationSyntax field) continue;
                var decalration = field.Declaration;
                var typeSyntax = decalration.Type;
                if (typeSyntax is not ArrayTypeSyntax arrayTypeSyntax) continue;
                if (!IsString(arrayTypeSyntax.ElementType)) continue;
                if (decalration.Variables.Count != 1) continue;
                var initializerSyntax = decalration.Variables.First().Initializer;
                if (initializerSyntax is null) continue;

                var arrayItems = initializerSyntax.Value.ChildNodes();
                foreach (var arrayItem in arrayItems) {
                    if (arrayItem is not LiteralExpressionSyntax literalSyntax) continue;
                    var feature = literalSyntax.ChildTokens().First().ValueText;
                    result.Featrues.Add(feature);
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