namespace StronglyTyped.FeatureFlags.Abstractions.Tests;

public class FeatureTests {
    private static Feature CreateFeature()
        => new("SomeName", FlagType.Transient, true);

    [Fact]
    public void Feature_Properties_Work() {
        // Arrange
        var feature = CreateFeature();

        // Act
        feature = feature with {
            Name = "OtherName",
            Type = FlagType.Static,
            IsEnabled = false,
        };

        // Assert
        feature.Name.Should().Be("OtherName");
        feature.Type.Should().Be(FlagType.Static);
        feature.IsEnabled.Should().Be(false);
    }
}

