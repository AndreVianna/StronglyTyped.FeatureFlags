namespace StronglyTyped.FeatureFlags.Providers.Configuration.Tests;

[ExcludeFromCodeCoverage]
public class ConfigurationFeatureProviderTests {
    private readonly IConfiguration _subConfiguration;

    public ConfigurationFeatureProviderTests() {
        _subConfiguration = new ConfigurationBuilder().AddJsonFile("testsettings.json").Build();
    }

    private ConfigurationFeatureProvider CreateProvider(string? section = null)
        => new(_subConfiguration, section);

    [Fact]
    public void GetAll_ReturnsAllFeatures() {
        // Arrange
        var provider = CreateProvider();
        var expectedFeatures = new[] {
            new Feature("Feature1", typeof(ConfigurationFeatureProvider), FeatureStateLifecycle.Static, true),
            new Feature("Feature2", typeof(ConfigurationFeatureProvider), FeatureStateLifecycle.Static, false),
            new Feature("Feature3", typeof(ConfigurationFeatureProvider), FeatureStateLifecycle.Scoped, true),
            new Feature("Feature4", typeof(ConfigurationFeatureProvider), FeatureStateLifecycle.Transient, true),
            new Feature("Feature5", typeof(ConfigurationFeatureProvider), FeatureStateLifecycle.Static, false),
            new Feature("Feature6", typeof(ConfigurationFeatureProvider), FeatureStateLifecycle.Static, false),
            new Feature("Feature7", typeof(ConfigurationFeatureProvider), FeatureStateLifecycle.Static, false)
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
            new Feature("FeatureA", typeof(ConfigurationFeatureProvider), FeatureStateLifecycle.Static, true),
            new Feature("FeatureB", typeof(ConfigurationFeatureProvider), FeatureStateLifecycle.Static, false),
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
    [InlineData("Feature5", FeatureStateLifecycle.Static, false)]
    [InlineData("Feature6", FeatureStateLifecycle.Static, false)]
    [InlineData("Feature7", FeatureStateLifecycle.Static, false)]
    public void GetByName_ForExistingFeature_ReturnsExpectedValue(string featureName, FeatureStateLifecycle expectedLifecycle, bool expectedState) {
        // Arrange
        var provider = CreateProvider();

        // Act
        var result = provider.GetByNameOrDefault(featureName);

        // Assert
        var subject = result.Should().BeOfType<Feature>().Subject;
        subject.Name.Should().Be(featureName);
        subject.Lifecycle.Should().Be(expectedLifecycle);
        subject.IsEnabled.Should().Be(expectedState);
    }


    [Fact]
    public void GetByName_ForNonExistingFeature_ReturnsNull() {
        // Arrange
        var provider = CreateProvider();

        // Act
        var result = provider.GetByNameOrDefault("Feature9");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetByName_ForNonExistingSection_ReturnsNull() {
        // Arrange
        var provider = CreateProvider("Invalid");

        // Act
        var result = provider.GetByNameOrDefault("Feature1");

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