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
    public void Name_ReturnsName() {
        // Arrange
        var provider = CreateProvider();

        // Act
        var result = provider.Name;

        // Assert
        result.Should().Be("Configuration");
    }

    [Fact]
    public void GetAll_ReturnsAllFeatures() {
        // Arrange
        var provider = CreateProvider();
        var expectedFeatures = new[] {
            new Feature("Feature1", FlagType.Static, true),
            new Feature("Feature2", FlagType.Static, false),
            new Feature("Feature3", FlagType.Transient, true),
            new Feature("Feature4", FlagType.Static, false),
            new Feature("Feature5", FlagType.Static, false),
            new Feature("Feature6", FlagType.Static, false)
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
            new Feature("FeatureA", FlagType.Static, true),
            new Feature("FeatureB", FlagType.Static, false),
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
    [InlineData("Feature1", FlagType.Static, true)]
    [InlineData("Feature2", FlagType.Static, false)]
    [InlineData("Feature3", FlagType.Transient, true)]
    [InlineData("Feature4", FlagType.Static, false)]
    [InlineData("Feature5", FlagType.Static, false)]
    [InlineData("Feature6", FlagType.Static, false)]
    public void GetByName_ForExistingFeature_ReturnsExpectedValue(string featureName, FlagType expectedType, bool expectedState) {
        // Arrange
        var provider = CreateProvider();

        // Act
        var result = provider.GetByNameOrDefault(featureName);

        // Assert
        var subject = result.Should().BeOfType<Feature>().Subject;
        subject.Name.Should().Be(featureName);
        subject.Type.Should().Be(expectedType);
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