namespace StronglyTyped.FeatureFlags.Abstractions.Tests;

[ExcludeFromCodeCoverage]
public class FeatureAccessDefinitionAttributeTests {
    [Fact]
    public void FeatureAccessDefinitionAttribute_GroupPath_IsInitializedFromConstructor() {
        // Arrange
        var attribute = new FeatureAccessDefinitionAttribute("Root", "Child");

        // Assert
        attribute.BasePath.Should().BeEquivalentTo("Root", "Child");
    }
}