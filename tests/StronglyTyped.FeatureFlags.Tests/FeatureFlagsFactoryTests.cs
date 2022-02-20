namespace StronglyTyped.FeatureFlags.Tests;

public class FeatureFlagsFactoryTests {

    private readonly ProcessSpy _processSpy = new();
    private readonly IServiceCollection _serviceCollection = Substitute.For<ServiceCollection>();

    public FeatureFlagsFactoryTests() {
        _serviceCollection.AddSingleton(_processSpy);
    }

    private FeatureFlagsFactory CreateFactory()
        => new(_serviceCollection);

    [Fact]
    public void AddProvider_ForNewProvider_RegistersTheProvider() {
        // Arrange
        var factory = CreateFactory();
        _processSpy.ClearCalls();

        // Act
        factory.AddProvider<FakeProvider>();

        // Assert
        _processSpy.GetCalls().Should().BeEquivalentTo("Constructor", "Name", "Name", "GetAll", "Name", "Name", "Name", "Dispose");
    }

    [Fact]
    public void AddProvider_ForNewProvider_WithConstructor_RegistersTheProvider() {
        // Arrange
        var factory = CreateFactory();
        _processSpy.ClearCalls();

        // Act
        factory.AddProvider(_ => new FakeProvider(_processSpy));

        // Assert
        _processSpy.GetCalls().Should().BeEquivalentTo("Constructor", "Name", "Name", "GetAll", "Name", "Name", "Name", "Dispose");
    }

    [Fact]
    public void AddProvider_ForRegisteredProvider_IgnoresDuplicatedProvider() {
        // Arrange
        var factory = CreateFactory();
        factory.AddProvider<FakeProvider>();
        _processSpy.ClearCalls();

        // Act
        factory.AddProvider<FakeProvider>();

        // Assert
        _processSpy.GetCalls().Should().BeEquivalentTo("Constructor", "Name", "Dispose");
    }


    [Fact]
    public void AddProvider_ForProviderWithDuplicatedFeature_Throws() {
        // Arrange
        var factory = CreateFactory();
        factory.AddProvider<FakeProvider>();
        _processSpy.ClearCalls();

        // Act
        var action = [ExcludeFromCodeCoverage] () => factory.AddProvider<FakeProviderWithDuplicatedFeature>();

        // Assert
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Duplicated feature flag definition found in 'FakeProvider' and 'FakeProviderWithDuplicatedFeature' providers.");
        _processSpy.GetCalls().Should().BeEquivalentTo("Constructor", "Name", "Name", "GetAll", "Name", "Dispose");
    }

    [Fact]
    public void For_ExistingStaticFeature_ReturnsFlag_WithoutCallingProvider() {
        // Arrange
        var factory = CreateFactory();
        factory.AddProvider<FakeProvider>();
        _processSpy.ClearCalls();
        const string name = "Feature1";

        // Act
        var result = factory.For(name);

        // Assert
        result.Should().NotBeOfType<Flag>();
        result.IsEnabled.Should().BeTrue();
        _processSpy.GetCalls().Should().BeEmpty();
    }

    [Fact]
    public void For_ExistingTransientFeature_CallsProvider_And_ReturnsFlag() {
        // Arrange
        var factory = CreateFactory();
        factory.AddProvider<FakeProvider>();
        _processSpy.ClearCalls();
        const string name = "Feature2";

        // Act
        var result = factory.For(name);

        // Assert
        result.Should().NotBeOfType<Flag>();
        result.IsEnabled.Should().BeTrue();
        _processSpy.GetCalls().Should().BeEquivalentTo("Constructor", "GetByNameOrDefault(Feature2)", "Dispose");
    }

    [Fact]
    public void For_NonRegisteredFeature_ReturnsNullFlag() {
        // Arrange
        var factory = CreateFactory();
        factory.AddProvider<FakeProvider>();
        _processSpy.ClearCalls();
        const string name = "Invalid";

        // Act
        var result = factory.For(name);

        // Assert
        result.Should().BeOfType<NullFlag>();
        result.IsEnabled.Should().BeFalse();
        _processSpy.GetCalls().Should().BeEmpty();
    }

    [Fact]
    public void For_RemoveTransientFeature_ReturnsNullFlag() {
        // Arrange
        var factory = CreateFactory();
        factory.AddProvider<FakeProvider>();
        _processSpy.ClearCalls();
        const string name = "Feature3";

        // Act
        var result = factory.For(name);

        // Assert
        result.Should().BeOfType<NullFlag>();
        result.IsEnabled.Should().BeFalse();
        _processSpy.GetCalls().Should().BeEquivalentTo("Constructor", "GetByNameOrDefault(Feature3)", "Dispose");
    }
}