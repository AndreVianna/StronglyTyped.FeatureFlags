namespace StronglyTyped.FeatureFlags.Abstractions.Tests;

[ExcludeFromCodeCoverage]
public class FeatureTests {
    [Fact]
    public void Feature_IsARecord() {
        // Arrange
        var feature = new Feature("SomeName", FlagType.Transient, false);

        // Act
        feature = feature with {
            Name = "OtherName",
            Type = FlagType.Static,
            IsEnabled = true,
        };

        // Assert
        feature.Name.Should().Be("OtherName");
        feature.Type.Should().Be(FlagType.Static);
        feature.IsEnabled.Should().BeTrue();
    }
}
