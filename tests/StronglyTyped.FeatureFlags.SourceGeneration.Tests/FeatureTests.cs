namespace StronglyTyped.FeatureFlags.SourceGeneration.Tests;

[ExcludeFromCodeCoverage]
public class FlagsSelectorTests {
    [Fact]
    public void Feature_Properties_Work() {
        // Arrange
        var selector = new FeatureAccessDefinition("SomeNamespace", "SomeName");

        // Assert
        selector.Namespace.Should().Be("SomeNamespace");
        selector.Name.Should().Be("SomeName");
        selector.Features.Should().BeEmpty();
    }
}

