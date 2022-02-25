namespace StronglyTyped.FeatureFlags.Abstractions.Tests;

[ExcludeFromCodeCoverage]
public class FeatureListAttributeTests {
    [Fact]
    public void FeatureListAttribute_IsAClass() {
        // Arrange
        var attribute = new FeatureListAttribute("SomeField");

        // Assert
        attribute.FieldName.Should().Be("SomeField");
    }
}