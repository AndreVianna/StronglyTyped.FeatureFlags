namespace StronglyTyped.FeatureFlags.SourceGeneration;

internal class Analyzer {
    private static readonly DiagnosticDescriptor _stringArrayFieldRule = new(
        "FG001",
        "Invalid field type.",
        "The selected field must be a string array.",
        "Code",
        DiagnosticSeverity.Warning,
        true,
        "The selected field must be a string array.");

    private static readonly DiagnosticDescriptor _privateFieldRule = new(
        "FG002",
        "Invalid field modifier.",
        "The selected field must be private.",
        "Code",
        DiagnosticSeverity.Warning,
        true,
        "The selected field must be private.");

    private static readonly DiagnosticDescriptor _featureValueRule = new(
        "FG003",
        "Invalid feature name definition.",
        "The feature name must be a string literal. Instead found '{0}'.",
        "Code",
        DiagnosticSeverity.Warning,
        true,
        "The feature name must be a string literal. Instead found '{0}'.");

    private static readonly DiagnosticDescriptor _sectionValueRule = new(
        "FG004",
        "Invalid section name definition.",
        "The section name must use 'nameof' keyword. Instead found '{0}'.",
        "Code",
        DiagnosticSeverity.Warning,
        true,
        "The section name must use 'nameof' keyword. Instead found '{0}'.");

    internal static ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        => ImmutableArray.Create(_stringArrayFieldRule, _privateFieldRule, _featureValueRule, _sectionValueRule);

    internal const string FeaturesAttributeType = "StronglyTyped.FeatureFlags.FeaturesAttribute";
    internal const string SectionsAttributeType = "StronglyTyped.FeatureFlags.SectionsAttribute";
    internal const string FeaturesSectionDefinitionAttributeType = "StronglyTyped.FeatureFlags.FeaturesSectionDefinitionAttribute";

    internal static void AnalyzeNode(SyntaxNodeAnalysisContext context) {
        var model = context.SemanticModel;
        var classDeclaration = (ClassDeclarationSyntax)context.Node;

        if (!HasAttribute(model, classDeclaration, FeaturesSectionDefinitionAttributeType)) return;

        foreach (var field in classDeclaration.Members.OfType<FieldDeclarationSyntax>()) {
            if (HasAttribute(model, field, FeaturesAttributeType)) {
                if (!IsFieldValid(field, context)) continue;
                foreach (var variable in field.Declaration.Variables.Where(v => v.Initializer is not null))
                    ValidateFeatures(context, variable);
            }
            else if (HasAttribute(model, field, SectionsAttributeType)) {
                if (!IsFieldValid(field, context)) continue;
                foreach (var variable in field.Declaration.Variables.Where(v => v.Initializer is not null))
                    ValidateSections(context, variable);
            }
        }
    }

    internal static bool HasAttribute(SemanticModel model, MemberDeclarationSyntax member, string attributeTypeName)
        => member.AttributeLists.SelectMany(i => i.Attributes).Any(attribute => ContainingTypeIs(model, attribute, attributeTypeName));

    internal static bool ContainingTypeIs(SemanticModel model, SyntaxNode attribute, string attributeTypeName) =>
        model.GetSymbolInfo(attribute).Symbol is IMethodSymbol attributeConstructor &&
        attributeConstructor.ContainingType.ToDisplayString() == attributeTypeName;

    internal static bool IsFieldValid(BaseFieldDeclarationSyntax field, SyntaxNodeAnalysisContext? context = null) {
        if (field.Declaration.Type is not ArrayTypeSyntax arrayType || !IsString(arrayType.ElementType)) {
            context?.ReportDiagnostic(Diagnostic.Create(_stringArrayFieldRule, field.Declaration.Type.GetLocation()));
            return false;
        }
        if (field.Modifiers.All(i => i.Text is not "private")) {
            context?.ReportDiagnostic(Diagnostic.Create(_privateFieldRule, field.Modifiers.First().GetLocation()));
            return false;
        }
        return true;
    }

    private static bool IsString(TypeSyntax type) {
        return type switch {
            PredefinedTypeSyntax pfs when pfs.ToString() == "string" => true,
            IdentifierNameSyntax ins when ins.ToString() == "String" => true,
            QualifiedNameSyntax qns when qns.ToString() == "System.String" => true,
            _ => false,
        };
    }

    private static void ValidateFeatures(SyntaxNodeAnalysisContext context, VariableDeclaratorSyntax variable) {
        var arrayItems = variable.Initializer!.Value.ChildNodes().ToArray();
        foreach (var arrayItem in arrayItems)
            IsFeatureValid(arrayItem, context);
    }

    internal static bool IsFeatureValid(SyntaxNode arrayItem, SyntaxNodeAnalysisContext? context = null) {
        if (arrayItem is LiteralExpressionSyntax) return true;
        context?.ReportDiagnostic(Diagnostic.Create(_featureValueRule, arrayItem.GetLocation(), arrayItem.ToString()));
        return false;

    }

    private static void ValidateSections(SyntaxNodeAnalysisContext context, VariableDeclaratorSyntax variable) {
        var arrayItems = variable.Initializer!.Value.ChildNodes();
        foreach (var arrayItem in arrayItems)
            IsSectionValid(arrayItem, context);
    }

    internal static bool IsSectionValid(SyntaxNode arrayItem, SyntaxNodeAnalysisContext? context = null) {
        if (arrayItem is InvocationExpressionSyntax {Expression: IdentifierNameSyntax}) return true;
        context?.ReportDiagnostic(Diagnostic.Create(_sectionValueRule, arrayItem.GetLocation(), arrayItem.ToString()));
        return false;
    }
}
