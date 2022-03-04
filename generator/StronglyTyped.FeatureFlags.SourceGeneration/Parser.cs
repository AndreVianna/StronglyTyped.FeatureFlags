namespace StronglyTyped.FeatureFlags.SourceGeneration;

internal static class Parser {

    internal static bool IsSyntaxTargetForGeneration(SyntaxNode node)
        => node is ClassDeclarationSyntax { AttributeLists.Count: > 0 };

    internal static SectionDefinition? CreateSectionDefinition(GeneratorSyntaxContext context) {
        var classDeclaration = (ClassDeclarationSyntax)context.Node;
        var model = context.SemanticModel;

        var featureSectionDefinition = CreateSectionDefinition(model, classDeclaration);
        if (featureSectionDefinition is null) return null;

        foreach (var field in classDeclaration.Members.OfType<FieldDeclarationSyntax>()) {
            if (Analyzer.HasAttribute(model, field, Analyzer.FeaturesAttributeType)) {
                if (!Analyzer.IsFieldValid(field)) continue;
                foreach (var variable in field.Declaration.Variables.Where(v => v.Initializer is not null))
                    AddValidFeatures(variable, featureSectionDefinition, GetPathFrom(model, variable, Analyzer.FeaturesAttributeType));
            }
            else if (Analyzer.HasAttribute(model, field, Analyzer.SectionsAttributeType)) {
                if (!Analyzer.IsFieldValid(field)) continue;
                foreach (var variable in field.Declaration.Variables.Where(v => v.Initializer is not null))
                    AddValidSections(variable, featureSectionDefinition);
            }
        }

        return featureSectionDefinition;

    }

    private static SectionDefinition? CreateSectionDefinition(SemanticModel model, BaseTypeDeclarationSyntax classDeclaration) {
        var (@namespace, className) = Parser.ExtractClassDefinition(classDeclaration);
        if (@namespace is null) return null;
        if (!Analyzer.HasAttribute(model, classDeclaration, Analyzer.FeaturesSectionDefinitionAttributeType)) return null;
        var basePath = GetPathFrom(model, classDeclaration, Analyzer.FeaturesSectionDefinitionAttributeType);
        return new SectionDefinition(@namespace, className, basePath);
    }

    private static (string?, string) ExtractClassDefinition(BaseTypeDeclarationSyntax classDeclaration) {
        var @namespace = classDeclaration.Parent switch {
            FileScopedNamespaceDeclarationSyntax namespaceSyntax => namespaceSyntax.Name.ToString(),
            NamespaceDeclarationSyntax namespaceSyntax => namespaceSyntax.Name.ToString(),
            _ => null
        };
        var name = classDeclaration.Identifier.ToString();
        return (@namespace, name);
    }

    private static void AddValidFeatures(VariableDeclaratorSyntax variable, SectionDefinition definition, IEnumerable<string> groupPath) {
        var path = groupPath.ToArray();
        var arrayItems = variable.Initializer!.Value.ChildNodes().ToArray();
        foreach (var arrayItem in arrayItems) AddValidFeature(definition, arrayItem, path);
    }

    private static void AddValidFeature(SectionDefinition definition, SyntaxNode arrayItem, IEnumerable<string> path) {
        if (!Analyzer.IsFeatureValid(arrayItem)) return;
        definition.Features.Add((path, ((LiteralExpressionSyntax)arrayItem).ChildTokens().First().ValueText));
    }

    private static void AddValidSections(VariableDeclaratorSyntax variable, SectionDefinition definition) {
        var arrayItems = variable.Initializer!.Value.ChildNodes();
        foreach (var arrayItem in arrayItems) AddValidSection(definition, arrayItem);
    }

    private static void AddValidSection(SectionDefinition definition, SyntaxNode arrayItem) {
        if (!Analyzer.IsSectionValid(arrayItem)) return;
        definition.Sections.Add(((InvocationExpressionSyntax)arrayItem).ArgumentList.Arguments[0].ToString());
    }

    private static IEnumerable<string> GetPathFrom(SemanticModel model, SyntaxNode member, string attributeTypeName) {
        var symbol = model.GetDeclaredSymbol(member)!;
        var constructorName = attributeTypeName.Split('.').Last();
        var boundAttributes = symbol.GetAttributes().First(i => i.AttributeClass!.Name == constructorName);
        return boundAttributes.ConstructorArguments.First().Values.OfType<string>().ToArray();
    }
}