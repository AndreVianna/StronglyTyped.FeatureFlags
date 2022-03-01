namespace StronglyTyped.FeatureFlags.Providers.Configuration.Tests;

[ExcludeFromCodeCoverage]
public class ConfigurationFeatureProviderTests {
    private readonly IConfiguration _subConfiguration;

    public ConfigurationFeatureProviderTests() {
        _subConfiguration = new ConfigurationBuilder().AddJsonFile("testsettings.json").Build();
    }

    private ConfigurationFeatureProvider CreateProvider(params string[] section)
        => new(_subConfiguration, section);

    [Fact]
    public void GetAll_ReturnsAllFeatures() {
        // Arrange
        var provider = CreateProvider();
        var expectedFeatures = new[] {
            new Feature(new[] { "Feature1" }, typeof(ConfigurationFeatureProvider), FeatureStateLifecycle.Static, true),
            new Feature(new[] { "Feature2" }, typeof(ConfigurationFeatureProvider), FeatureStateLifecycle.Static, false),
            new Feature(new[] { "Feature3" }, typeof(ConfigurationFeatureProvider), FeatureStateLifecycle.Scoped, true),
            new Feature(new[] { "Feature4" }, typeof(ConfigurationFeatureProvider), FeatureStateLifecycle.Transient, true),
            new Feature(new[] { "SubFeatures", "Feature101" }, typeof(ConfigurationFeatureProvider), FeatureStateLifecycle.Static, true),
            new Feature(new[] { "SubFeatures", "Feature102" }, typeof(ConfigurationFeatureProvider), FeatureStateLifecycle.Static, false),
        };

        // Act
        var result = provider.GetAll();

        // Assert
        result.Should().BeEquivalentTo(expectedFeatures);
    }

    [Fact]
    public void GetAll_WithAlternativeSection_ReturnsOtherFeatures() {
        // Arrange
        var provider = CreateProvider("OtherFeatures");
        var expectedFeatures = new[] {
            new Feature(new[] { "FeatureA" }, typeof(ConfigurationFeatureProvider), FeatureStateLifecycle.Static, true),
            new Feature(new[] { "FeatureB" }, typeof(ConfigurationFeatureProvider), FeatureStateLifecycle.Static, false),
        };

        // Act
        var result = provider.GetAll();

        // Assert
        result.Should().BeEquivalentTo(expectedFeatures);
    }

    [Fact]
    public void GetAll_WithNonExistingSection_ReturnsEmpty() {
        // Arrange
        var provider = CreateProvider("Invalid");

        // Act
        var result = provider.GetAll();

        // Assert
        result.Should().BeEmpty();
    }

    [Theory]
    [InlineData("Feature1", FeatureStateLifecycle.Static, true)]
    [InlineData("Feature2", FeatureStateLifecycle.Static, false)]
    [InlineData("Feature3", FeatureStateLifecycle.Scoped, true)]
    [InlineData("Feature4", FeatureStateLifecycle.Transient, true)]
    public void GetFromPath_ForExistingFeature_ReturnsExpectedValue(string featureName, FeatureStateLifecycle expectedLifecycle, bool expectedState) {
        // Arrange
        var provider = CreateProvider();

        // Act
        var result = provider.GetFromPathOrDefault(featureName);

        // Assert
        var subject = result.Should().BeOfType<Feature>().Subject;
        subject.Path.Should().BeEquivalentTo(featureName);
        subject.Lifecycle.Should().Be(expectedLifecycle);
        subject.IsEnabled.Should().Be(expectedState);
    }


    [Theory]
    [InlineData("Feature5")]
    [InlineData("Feature6")]
    [InlineData("Feature7")]
    [InlineData("Feature9")]
    public void GetFromPath_ForNonExistingFeature_ReturnsNull(string featureName) {
        // Arrange
        var provider = CreateProvider();

        // Act
        var result = provider.GetFromPathOrDefault(featureName);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetFromPath_ForNonExistingSection_ReturnsNull() {
        // Arrange
        var provider = CreateProvider("Invalid");

        // Act
        var result = provider.GetFromPathOrDefault("Feature1");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Dispose_DoesNothing() {
        // Arrange
        var provider = CreateProvider();

        // Act
        provider.Dispose();
    }
}