namespace StronglyTyped.FeatureFlags.SourceGeneration;

internal class Parser {
    internal static bool IsSyntaxTargetForGeneration(SyntaxNode node) =>
        node is ClassDeclarationSyntax { AttributeLists.Count: > 0 };

    internal static SectionDefinition? GetSemanticTargetForGeneration(GeneratorSyntaxContext context) {
        var classDeclaration = (ClassDeclarationSyntax)context.Node;
        if (!TryCreateSectionDefinition(context, classDeclaration, out var featureSectionDefinition)) return null;

        foreach (var field in classDeclaration.Members.OfType<FieldDeclarationSyntax>()) {
            if (HasAttribute<FeaturesAttribute>(context, field)) {
                if (!IsFieldValid(field, featureSectionDefinition)) continue;
                foreach (var variable in field.Declaration.Variables.Where(v => v.Initializer is not null))
                    AddFeatures(variable, featureSectionDefinition, GetPathFrom<FeaturesAttribute>(context, variable));
            }
            else if (HasAttribute<SectionsAttribute>(context, field)) {
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

    private static bool HasAttribute<T>(GeneratorSyntaxContext context, MemberDeclarationSyntax member)
        => member.AttributeLists.SelectMany(i => i.Attributes).Any(attribute => ContainingTypeIs<T>(context, attribute));

    private static bool ContainingTypeIs<T>(GeneratorSyntaxContext context, SyntaxNode attribute) =>
        context.SemanticModel.GetSymbolInfo(attribute).Symbol is IMethodSymbol attributeConstructor &&
        attributeConstructor.ContainingType.ToDisplayString() == typeof(T).FullName;

    private static IEnumerable<string> GetPathFrom<T>(GeneratorSyntaxContext context, SyntaxNode member) {
        var symbol = context.SemanticModel.GetDeclaredSymbol(member)!;
        var constructorName = typeof(T).FullName.Split('.').Last();
        var boundAttributes = symbol.GetAttributes().First(i => i.AttributeClass!.Name == constructorName);
        return boundAttributes.ConstructorArguments.First().Values.OfType<string>().ToArray();
    }

    private static bool TryCreateSectionDefinition(GeneratorSyntaxContext context, BaseTypeDeclarationSyntax classDeclaration, out SectionDefinition sectionDefinition) {
        sectionDefinition = default!;
        var (@namespace, className) = Parser.ExtractClassDefinition(classDeclaration);
        if (@namespace is null) return false;
        if (!HasAttribute<FeaturesSectionDefinitionAttribute>(context, classDeclaration)) return false;
        var basePath = GetPathFrom<FeaturesSectionDefinitionAttribute>(context, classDeclaration);
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