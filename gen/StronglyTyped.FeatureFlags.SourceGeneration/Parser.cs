namespace StronglyTyped.FeatureFlags.SourceGeneration;

internal class Parser {
    internal static bool IsSyntaxTargetForGeneration(SyntaxNode node) =>
        node is ClassDeclarationSyntax { AttributeLists.Count: > 0 };

    internal static SectionDefinition? GetSemanticTargetForGeneration(GeneratorSyntaxContext context) {
        var classDeclaration = (ClassDeclarationSyntax)context.Node;
        if (!TryCreateSectionDefinition(context, classDeclaration, out var featureSectionDefinition)) return null;

        foreach (var field in classDeclaration.Members.OfType<FieldDeclarationSyntax>()) {
            if (HasAttribute<FeaturesAttribute>(context, field)) {
                ValidateField(field);
                foreach (var variable in field.Declaration.Variables.Where(v => v.Initializer is not null))
                    AddFeatures(variable, featureSectionDefinition, GetPathFrom<FeaturesAttribute>(context, variable));
            }
            else if (HasAttribute<SectionsAttribute>(context, field)) {
                ValidateField(field);
                foreach (var variable in field.Declaration.Variables.Where(v => v.Initializer is not null))
                    AddSections(variable, featureSectionDefinition);
            }
        }
        return featureSectionDefinition;

    }

    private static void AddFeatures(VariableDeclaratorSyntax variable, SectionDefinition definition, IEnumerable<string> groupPath) {
        var arrayItems = variable.Initializer!.Value.ChildNodes();
        foreach (var arrayItem in arrayItems) {
            if (arrayItem is LiteralExpressionSyntax literal)
                definition.Features.Add((groupPath, literal.ChildTokens().First().ValueText));
        }
    }

    private static void AddSections(VariableDeclaratorSyntax variable, SectionDefinition definition) {
        var arrayItems = variable.Initializer!.Value.ChildNodes();
        foreach (var arrayItem in arrayItems) {
            if (arrayItem is InvocationExpressionSyntax {Expression: IdentifierNameSyntax} invocation)
                definition.Sections.Add(invocation.ArgumentList.Arguments[0].ToString());
        }
    }

    private static bool HasAttribute<T>(GeneratorSyntaxContext context, MemberDeclarationSyntax member)
        => member.AttributeLists.SelectMany(i => i.Attributes).Any(attribute => ContainingTypeIs<T>(context, attribute));

    private static bool ContainingTypeIs<T>(GeneratorSyntaxContext context, SyntaxNode attribute) =>
        context.SemanticModel.GetSymbolInfo(attribute).Symbol is IMethodSymbol attributeConstructor &&
        attributeConstructor.ContainingType.ToDisplayString() == typeof(T).FullName;

    private static IEnumerable<string> GetPathFrom<T>(GeneratorSyntaxContext context, SyntaxNode member) {
        var symbol = context.SemanticModel.GetDeclaredSymbol(member)!;
        var constructorName = typeof(T).FullName.Split('.').Last();
        var boundAttributes = symbol.GetAttributes().First(i => i.AttributeClass!.Name == constructorName);
        return boundAttributes.ConstructorArguments.First().Values.Select(i => i.Value).Cast<string>().ToArray();
    }

    private static bool TryCreateSectionDefinition(GeneratorSyntaxContext context, ClassDeclarationSyntax classDeclaration, out SectionDefinition sectionDefinition) {
        sectionDefinition = default!;
        var (@namespace, className) = Parser.ExtractClassDefinition(classDeclaration);
        if (@namespace is null) return false;
        if (!HasAttribute<FeaturesSectionDefinitionAttribute>(context, classDeclaration)) return false;
        var basePath = GetPathFrom<FeaturesSectionDefinitionAttribute>(context, classDeclaration);
        sectionDefinition = new SectionDefinition(@namespace, className, basePath);
        return true;
    }

    private static void ValidateField(FieldDeclarationSyntax field) {
        if (field.Declaration.Type is not ArrayTypeSyntax arrayType || !IsString(arrayType.ElementType))
            throw new InvalidOperationException($"The feature group field must be a string array.");
        if (field.Modifiers.All(i => i.Text is not "private"))
            throw new InvalidOperationException($"The feature group must be private.");
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