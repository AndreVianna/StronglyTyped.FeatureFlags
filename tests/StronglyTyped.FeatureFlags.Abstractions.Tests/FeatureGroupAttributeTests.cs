namespace StronglyTyped.FeatureFlags.Abstractions.Tests;

[ExcludeFromCodeCoverage]
public class FeatureGroupAttributeTests {
    [Fact]
    public void FeatureGroupAttribute_GroupPath_IsInitializedFromConstructor() {
        // Arrange
        var attribute = new FeatureGroupAttribute("Root", "Child");

        // Assert
        attribute.GroupPath.Should().BeEquivalentTo("Root", "Child");
    }
}