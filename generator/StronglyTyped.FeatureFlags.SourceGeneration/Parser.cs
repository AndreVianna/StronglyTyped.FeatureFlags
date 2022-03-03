namespace StronglyTyped.FeatureFlags.SourceGeneration;

internal class Parser {

    private const string _featuresAttribute = "StronglyTyped.FeatureFlags.FeaturesAttribute";
    private const string _sectionsAttribute = "StronglyTyped.FeatureFlags.SectionsAttribute";
    private const string _featuresSectionsDefinitionAttribute = "StronglyTyped.FeatureFlags.FeaturesSectionDefinitionAttribute";

    internal static bool IsSyntaxTargetForGeneration(SyntaxNode node) =>
        node is ClassDeclarationSyntax { AttributeLists.Count: > 0 };

    internal static SectionDefinition? GetSemanticTargetForGeneration(GeneratorSyntaxContext context) {
        var classDeclaration = (ClassDeclarationSyntax)context.Node;
        if (!TryCreateSectionDefinition(context, classDeclaration, out var featureSectionDefinition)) return null;

        foreach (var field in classDeclaration.Members.OfType<FieldDeclarationSyntax>()) {
            if (HasAttribute(context, field, _featuresAttribute)) {
                if (!IsFieldValid(field, featureSectionDefinition)) continue;
                foreach (var variable in field.Declaration.Variables.Where(v => v.Initializer is not null))
                    AddFeatures(variable, featureSectionDefinition, GetPathFrom(context, variable, _featuresAttribute));
            }
            else if (HasAttribute(context, field, _sectionsAttribute)) {
                if (!IsFieldValid(field, featureSectionDefinition)) continue;
                foreach (var variable in field.Declaration.Variables.Where(v => v.Initializer is not null))
                    AddSections(variable, featureSectionDefinition);
            }
        }
        return featureSectionDefinition;

    }

    private static void AddFeatures(VariableDeclaratorSyntax variable, SectionDefinition definition, IEnumerable<string> groupPath) {
        var path = groupPath.ToArray();
        var arrayItems = variable.Initializer!.Value.ChildNodes().ToArray();
        foreach (var arrayItem in arrayItems)
            AddFeatureOrProblem(definition, arrayItem, path);
    }

    private static void AddFeatureOrProblem(SectionDefinition definition, SyntaxNode arrayItem, IEnumerable<string> path) {
        if (arrayItem is LiteralExpressionSyntax literal) {
            definition.Features.Add((path, literal.ChildTokens().First().ValueText));
            return;
        }

        var warning = CreateWarning(
            "FG003",
            "Invalid feature name definition.",
            "The feature name must be a string literal. Instead found '{0}'.");
        definition.Problems.Add((Diagnostic.Create(warning, arrayItem.GetLocation(), arrayItem.ToString()), false));
    }

    private static void AddSections(VariableDeclaratorSyntax variable, SectionDefinition definition) {
        var arrayItems = variable.Initializer!.Value.ChildNodes();
        foreach (var arrayItem in arrayItems)
            AddSectionOrProblem(definition, arrayItem);
    }

    private static void AddSectionOrProblem(SectionDefinition definition, SyntaxNode arrayItem) {
        if (arrayItem is InvocationExpressionSyntax { Expression: IdentifierNameSyntax } invocation) {
            definition.Sections.Add(invocation.ArgumentList.Arguments[0].ToString());
            return;
        }

        var warning = CreateWarning(
            "FG004", 
            "Invalid section name definition.", 
            "The section name must use 'nameof' keyword. Instead found '{0}'.");
        definition.Problems.Add((Diagnostic.Create(warning, arrayItem.GetLocation(), arrayItem.ToString()), false));
    }

    private static DiagnosticDescriptor CreateWarning(string id, string title, string message)
        => new(id, title, message, "Code", DiagnosticSeverity.Warning, true, message);

    private static bool HasAttribute(GeneratorSyntaxContext context, MemberDeclarationSyntax member, string attributeTypeName)
        => member.AttributeLists.SelectMany(i => i.Attributes).Any(attribute => ContainingTypeIs(context, attribute, attributeTypeName));

    private static bool ContainingTypeIs(GeneratorSyntaxContext context, SyntaxNode attribute, string attributeTypeName) =>
        context.SemanticModel.GetSymbolInfo(attribute).Symbol is IMethodSymbol attributeConstructor &&
        attributeConstructor.ContainingType.ToDisplayString() == attributeTypeName;

    private static IEnumerable<string> GetPathFrom(GeneratorSyntaxContext context, SyntaxNode member, string attributeTypeName) {
        var symbol = context.SemanticModel.GetDeclaredSymbol(member)!;
        var constructorName = attributeTypeName.Split('.').Last();
        var boundAttributes = symbol.GetAttributes().First(i => i.AttributeClass!.Name == constructorName);
        return boundAttributes.ConstructorArguments.First().Values.OfType<string>().ToArray();
    }

    private static bool TryCreateSectionDefinition(GeneratorSyntaxContext context, BaseTypeDeclarationSyntax classDeclaration, out SectionDefinition sectionDefinition) {
        sectionDefinition = default!;
        var (@namespace, className) = Parser.ExtractClassDefinition(classDeclaration);
        if (@namespace is null) return false;
        if (!HasAttribute(context, classDeclaration, _featuresSectionsDefinitionAttribute)) return false;
        var basePath = GetPathFrom(context, classDeclaration, _featuresSectionsDefinitionAttribute);
        sectionDefinition = new SectionDefinition(@namespace, className, basePath);
        return true;
    }

    private static bool IsFieldValid(BaseFieldDeclarationSyntax field, SectionDefinition definition) {
        var foundError = false;
        if (field.Declaration.Type is not ArrayTypeSyntax arrayType || !IsString(arrayType.ElementType)) {
            var warning = CreateWarning(
                "FG001", 
                "Invalid field type.", 
                "The selected field must be a string array.");
            definition.Problems.Add((Diagnostic.Create(warning, field.Declaration.GetLocation()), true));
            foundError = true;
        }
        if (field.Modifiers.All(i => i.Text is not "private")) {
            var warning = CreateWarning(
                "FG002", 
                "Invalid field modifier.",
                "The selected field must be private.");
            definition.Problems.Add((Diagnostic.Create(warning, field.Modifiers.First().GetLocation()), true));
            foundError = true;
        }
        return !foundError;
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