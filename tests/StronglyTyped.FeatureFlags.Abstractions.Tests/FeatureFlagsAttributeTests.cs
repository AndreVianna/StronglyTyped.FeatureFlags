namespace StronglyTyped.FeatureFlags.Abstractions.Tests;

[ExcludeFromCodeCoverage]
public class FeatureFlagsAttributeTests {
    [Fact]
    public void FeatureFlagsAttribute_IsAClass() {
        // Arrange
        var attribute = new FeatureFlagsAttribute("SomeField");

        // Assert
        attribute.FieldName.Should().Be("SomeField");
    }
}