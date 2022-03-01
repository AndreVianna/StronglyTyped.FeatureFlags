namespace StronglyTyped.FeatureFlags.SourceGeneration.Tests;

[ExcludeFromCodeCoverage]
public class FeaturesSectionDefinitionTests {
    [Fact]
    public void FeaturesSectionDefinition_Properties_AreDefinedInTheConstructor() {
        // Act
        var definition = new FeaturesSectionDefinition("SomeNamespace", "SomeName", new[] { "Root", "Child" });

        // Assert
        definition.Namespace.Should().Be("SomeNamespace");
        definition.ClassName.Should().Be("SomeName");
        definition.Path.Should().BeEquivalentTo("Root", "Child");
        definition.Features.Should().BeEmpty();
        definition.Sections.Should().BeEmpty();
    }
}