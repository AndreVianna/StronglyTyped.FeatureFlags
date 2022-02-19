namespace StronglyTyped.FeatureFlags.Abstractions.Tests;

public class FlagTests {
    private static Flag CreateFlag()
        => new(true);

    [Fact]
    public void Flag_Properties_Work() {
        // Arrange
        var flag = CreateFlag();

        // Act
        flag = flag with {
            IsEnabled = false,
        };

        // Assert
        flag.IsEnabled.Should().Be(false);
    }

    [Fact]
    public void NullFlag_Work() {
        // Act
        var flag = Flag.NullFlag;

        // Assert
        flag.IsEnabled.Should().BeFalse();
    }
}

