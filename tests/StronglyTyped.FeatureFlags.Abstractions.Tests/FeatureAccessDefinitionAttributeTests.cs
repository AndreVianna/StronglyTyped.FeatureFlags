namespace StronglyTyped.FeatureFlags.Abstractions.Tests;

[ExcludeFromCodeCoverage]
public class FeatureAccessDefinitionAttributeTests {
    [Fact]
    public void FeatureAccessDefinitionAttribute_IsAClass() {
        // Arrange
        var attribute = new FeatureAccessDefinitionAttribute("SomeField");

        // Assert
        attribute.FieldName.Should().Be("SomeField");
    }
}