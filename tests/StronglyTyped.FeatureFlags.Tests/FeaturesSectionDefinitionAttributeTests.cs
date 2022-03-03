namespace StronglyTyped.FeatureFlags.Tests;

[ExcludeFromCodeCoverage]
public class FeaturesSectionDefinitionAttributeTests {
    [Fact]
    public void FeaturesSectionDefinitionAttribute_GroupPath_IsInitializedFromConstructor() {
        // Arrange
        var attribute = new FeaturesSectionDefinitionAttribute("Root", "Child");

        // Assert
        attribute.BasePath.Should().BeEquivalentTo("Root", "Child");
    }
}