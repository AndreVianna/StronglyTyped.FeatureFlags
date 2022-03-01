namespace StronglyTyped.FeatureFlags.Abstractions.Tests;

[ExcludeFromCodeCoverage]
public class FeatureTests {
    [Fact]
    public void Feature_IsARecord() {
        // Arrange
        var feature = new Feature(new[] { "SomeName" }, typeof(string), FeatureStateLifecycle.Transient, false);

        // Act
        feature = feature with {
            Path = new[] { "OtherName" },
            ProviderType = typeof(int),
            Lifecycle = FeatureStateLifecycle.Static,
            IsEnabled = true,
        };

        // Assert
        feature.Path.Should().BeEquivalentTo("OtherName");
        feature.ProviderType.Should().Be(typeof(int));
        feature.Lifecycle.Should().Be(FeatureStateLifecycle.Static);
        feature.IsEnabled.Should().BeTrue();
    }
}
