namespace StronglyTyped.FeatureFlags.Tests;

[ExcludeFromCodeCoverage]
public class FeatureEntityTests {
    [Fact]
    public void FeatureEntity_IsARecord() {
        // Arrange
        var entity = new FeatureEntity("SomeName", FlagType.Transient, typeof(int));

        // Act
        entity = entity with {
            Name = "OtherName",
            FlagType = FlagType.Static,
            ProviderType = typeof(string),
        };

        // Assert
        entity.Name.Should().Be("OtherName");
        entity.FlagType.Should().Be(FlagType.Static);
        entity.ProviderType.Should().Be(typeof(string));
    }
}