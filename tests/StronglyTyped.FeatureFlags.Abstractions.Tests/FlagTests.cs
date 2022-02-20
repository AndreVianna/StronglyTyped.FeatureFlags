namespace StronglyTyped.FeatureFlags.Abstractions.Tests;

public class FlagTests {
    [Fact]
    public void Flag_Properties_Work() {
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
    public void NullFlag_Properties_Work() {
        // Arrange
        var flag = new NullFlag();

        // Assert
        flag.IsEnabled.Should().Be(false);
    }
}

