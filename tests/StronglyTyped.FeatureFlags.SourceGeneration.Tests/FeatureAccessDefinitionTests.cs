namespace StronglyTyped.FeatureFlags.SourceGeneration.Tests;

[ExcludeFromCodeCoverage]
public class FeatureGroupTests {
    [Fact]
    public void FeatureGroup_Properties_AreDefinedInTheConstructor() {
        // Act
        var group = new FeatureGroup(new[] { "Root", "Child" });

        // Assert
        group.Path.Should().BeEquivalentTo("Root", "Child");
        group.Features.Should().BeEmpty();
        group.Sections.Should().BeEmpty();
    }
}

[ExcludeFromCodeCoverage]
public class FeatureAccessDefinitionTests {
    [Fact]
    public void FeatureAccessDefinition_Properties_AreDefinedInTheConstructor() {
        // Act
        var definition = new FeatureAccessDefinition("SomeNamespace", "SomeName", new[] { "Root", "Child" });

        // Assert
        definition.Namespace.Should().Be("SomeNamespace");
        definition.ClassName.Should().Be("SomeName");
        definition.Path.Should().BeEquivalentTo("Root", "Child");
        definition.Groups.Should().BeEmpty();
    }
}
