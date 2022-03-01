namespace StronglyTyped.FeatureFlags.SourceGeneration;

internal class Parser {
    private const string _accessDefinitionAttributeType = "StronglyTyped.FeatureFlags.FeatureAccessDefinitionAttribute";
    private const string _groupAttributeType = "StronglyTyped.FeatureFlags.FeatureGroupAttribute";

    internal static bool IsSyntaxTargetForGeneration(SyntaxNode node) =>
        node is ClassDeclarationSyntax { AttributeLists.Count: > 0 };

    internal static FeatureAccessDefinition? GetSemanticTargetForGeneration(GeneratorSyntaxContext context) {
        var classDeclaration = (ClassDeclarationSyntax)context.Node;
        if (classDeclaration.AttributeLists.SelectMany(i => i.Attributes).All(attribute => !IsOfType(attribute, _accessDefinitionAttributeType))) return null;
        if (!TryGetPathFor(classDeclaration, _accessDefinitionAttributeType, out var basePath)) return null;
        var (@namespace, className) = Parser.ExtractClassDefinition(classDeclaration);
        if (@namespace is null) return null;

        var featureAccessDefinition = new FeatureAccessDefinition(@namespace, className, basePath);
        foreach (var field in classDeclaration.Members.OfType<FieldDeclarationSyntax>()) {
            if (field.AttributeLists.SelectMany(i => i.Attributes).All(attribute => !IsOfType(attribute, _groupAttributeType))) continue;

            if (field.Declaration.Type is not ArrayTypeSyntax arrayType || !IsString(arrayType.ElementType))
                throw new InvalidOperationException($"The feature group field must be a string array.");
            if (field.Modifiers.All(i => i.Text is not "private" and not "internal"))
                throw new InvalidOperationException($"The feature group must be private or internal.");

            foreach (var variable in field.Declaration.Variables.Where(v => v.Initializer is not null)) {
                if (!TryGetPathFor(variable, _groupAttributeType, out var groupPath)) continue;
                var group = new FeatureGroup(groupPath);
                var arrayItems = variable.Initializer!.Value.ChildNodes();
                foreach (var arrayItem in arrayItems) {
                    switch (arrayItem) {
                        case LiteralExpressionSyntax literal: {
                                var feature = literal.ChildTokens().First().ValueText;
                                group.Features.Add(feature);
                                break;
                            }
                        case InvocationExpressionSyntax { Expression: IdentifierNameSyntax } invocation: {
                                var section = invocation.ArgumentList.Arguments[0].ToString();
                                group.Sections.Add(section);
                                break;
                            }
                    }
                }
                featureAccessDefinition.Groups.Add(group);
            }
        }
        return featureAccessDefinition;

        bool IsOfType(SyntaxNode attribute, string attributeType) =>
            context.SemanticModel.GetSymbolInfo(attribute).Symbol is IMethodSymbol attributeConstructor &&
            attributeConstructor.ContainingType.ToDisplayString() == attributeType;

        bool TryGetPathFor(SyntaxNode member, string attributeType, out string[] path) {
            path = Array.Empty<string>();
            var symbol = context.SemanticModel.GetDeclaredSymbol(member)!;
            var constructorName = attributeType.Split('.').Last();
            var boundAttributes = symbol.GetAttributes().First(i => i.AttributeClass!.Name == constructorName);
            path = boundAttributes.ConstructorArguments.First().Values.Select(i => i.Value).Cast<string>().ToArray();
            return true;
        }
    }

    internal static (string?, string) ExtractClassDefinition(BaseTypeDeclarationSyntax classDeclaration) {
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