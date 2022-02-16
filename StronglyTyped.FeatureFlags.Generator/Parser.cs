namespace StronglyTyped.FeatureFlags.Generator;

internal class Parser {
    private const string _flagsHolderAttribute = "StronglyTyped.FeatureFlags.FeatureFlagsHolderAttribute";

    private readonly Compilation _compilation;
    private readonly Action<Diagnostic> _reportDiagnostic;
    private readonly CancellationToken _cancellationToken;

    public Parser(Compilation compilation, Action<Diagnostic> reportDiagnostic, CancellationToken cancellationToken) {
        _compilation = compilation;
        _reportDiagnostic = reportDiagnostic;
        _cancellationToken = cancellationToken;
    }

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

    internal class FlagsHolder {
        public FlagsHolder(string @namespace, string name) {
            Namespace = @namespace;
            Name = name;
        }
        public object Namespace { get; }
        public string Name { get; }
        public ICollection<string> Featrues { get; } = new HashSet<string>();
    }
    internal class FlagsDescriptor {
        public FlagsDescriptor(string name, string provider) {
            Name = name;
            Provider = provider;
        }
        public string Name { get; }
        public string Provider { get; }
    };

    public IReadOnlyList<FlagsHolder> GetFlagsHolderClasses(IEnumerable<ClassDeclarationSyntax> classes) {
#if DEBUG
        if (!Debugger.IsAttached) {
            Debugger.Launch();
        }
#endif
        var results = new List<FlagsHolder>();
        foreach (var group in classes.GroupBy(x => x.SyntaxTree)) {
            foreach (var classDeclaration in group) {
                var (@namespace, name) = ExtractClassDefinition(classDeclaration);
                if (@namespace == string.Empty) continue;
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
        }

        return results;
    }

    private static (string, string) ExtractClassDefinition(ClassDeclarationSyntax classDeclaration) {
        var @namespace = classDeclaration.Parent switch {
            FileScopedNamespaceDeclarationSyntax namespaceSyntax => namespaceSyntax.Name.ToString(),
            NamespaceDeclarationSyntax namespaceSyntax => namespaceSyntax.Name.ToString(),
            _ => string.Empty
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