namespace StronglyTyped.FeatureFlags.SourceGeneration.Tests;

public class FlagsSelectorTests {
    [Fact]
    public void Feature_Properties_Work() {
        // Arrange
        var selector = new FlagsSelector("SomeNamespace", "SomeName");

        // Act
        selector = selector with {
            Namespace = "OtherNamespace",
            Name = "OtherName",
        };

        // Assert
        selector.Namespace.Should().Be("OtherNamespace");
        selector.Name.Should().Be("OtherName");
    }
}

