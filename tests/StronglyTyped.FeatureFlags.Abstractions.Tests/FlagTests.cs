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
        flag.IsEnabled.Should().BeFalse();
    }

    [Fact]
    public void Flag_Null_IsNotEnabled() {
        // Arrange
        var flag = Flag.Null;

        // Assert
        flag.IsEnabled.Should().BeFalse();
    }
}

