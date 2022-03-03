namespace StronglyTyped.FeatureFlags.Tests;

[ExcludeFromCodeCoverage]
public class FeaturesAttributeTests {
    [Fact]
    public void FeaturesAttribute_GroupPath_IsInitializedFromConstructor() {
        // Arrange
        var attribute = new FeaturesAttribute("Root", "Child");

        // Assert
        attribute.Path.Should().BeEquivalentTo("Root", "Child");
    }
}