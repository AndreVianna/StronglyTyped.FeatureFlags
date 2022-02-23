namespace StronglyTyped.FeatureFlags.SourceGeneration;


internal class Parser {
    private const string _featureFlagsAttributeName = "FeatureFlagsAttribute";
    private const string _featureFlagsAttribute = "StronglyTyped.FeatureFlags.FeatureFlagsAttribute";

    internal static bool IsSyntaxTargetForGeneration(SyntaxNode node) =>
        node is ClassDeclarationSyntax { AttributeLists.Count: > 0 };

    internal static (ClassDeclarationSyntax ClassDeclaration, string FieldName)? GetSemanticTargetForGeneration(GeneratorSyntaxContext context) {
        var classDeclaration = (ClassDeclarationSyntax)context.Node;
        var classSymbol = (INamedTypeSymbol)context.SemanticModel.GetDeclaredSymbol(classDeclaration)!;

        foreach (var attributeList in classDeclaration.AttributeLists)
            foreach (var attribute in attributeList.Attributes) {
                var attributeConstructor = context.SemanticModel.GetSymbolInfo(attribute).Symbol as IMethodSymbol;
                if (attributeConstructor is not null && IsFromFeatureFlagsAttribute(attributeConstructor)) {
                    var boundAttributes = classSymbol.GetAttributes().First(i => i.AttributeClass!.Name == _featureFlagsAttributeName);
                    var fieldName = (string)boundAttributes.ConstructorArguments.First().Value!;
                    return (classDeclaration, fieldName);
                }
            }

        return null;

        static bool IsFromFeatureFlagsAttribute(ISymbol attributeConstructor) 
            => attributeConstructor.ContainingType.ToDisplayString() == _featureFlagsAttribute;
    }

    public IReadOnlyList<FlagsSelector> GetFlagsSelectors(IEnumerable<(ClassDeclarationSyntax ClassDeclaration, string FieldName)> selectors, CancellationToken cancellationToken) {
        var results = new List<FlagsSelector>();
        foreach (var (classDeclaration, fieldName) in selectors) {
            cancellationToken.ThrowIfCancellationRequested();
            var (@namespace, name) = ExtractClassDefinition(classDeclaration);
            if (@namespace is null) continue;
            var result = new FlagsSelector(@namespace, name);
            var field = classDeclaration
                .Members
                .OfType<FieldDeclarationSyntax>()
                .FirstOrDefault(f => f.Declaration.Variables.Any(i => i.Identifier.Text == fieldName));
            if (field is null)
                throw new InvalidOperationException($"A field with name '{fieldName}' was not found.");
            if (field.Declaration.Type is not ArrayTypeSyntax arrayType || !IsString(arrayType.ElementType))
                throw new InvalidOperationException($"The '{fieldName}' field must be a string array.");
            if (field.Modifiers.All(i => i.Text != "private"))
                throw new InvalidOperationException($"The '{fieldName}' must be private.");

            results.Add(result);
            var variable = field.Declaration.Variables.First(i => i.Identifier.Text == fieldName);
            var initializer = variable.Initializer;
            if (initializer is null) continue;
            var arrayItems = initializer.Value.ChildNodes();
            foreach (var arrayItem in arrayItems) {
                if (arrayItem is not LiteralExpressionSyntax literalSyntax) continue;
                var feature = literalSyntax.ChildTokens().First().ValueText;
                result.Features.Add(feature);
            }
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