namespace StronglyTyped.FeatureFlags.Abstractions.Tests;

[ExcludeFromCodeCoverage]
public class FlagTests {
    [Fact]
    public void Flag_IsARecord() {
        // Arrange
        var flag = new Flag(true);

        // Act
        flag = flag with {
            IsEnabled = false,
        };

        // Assert
        flag.IsEnabled.Should().Be(false);
    }

    [Fact]
    public void NullFlag_IsARecord() {
        // Arrange
        var flag = new NullFlag();

        // Assert
        flag.IsEnabled.Should().Be(false);
    }
}

