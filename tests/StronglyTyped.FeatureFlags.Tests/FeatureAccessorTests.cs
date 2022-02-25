namespace StronglyTyped.FeatureFlags.Tests;
using TestDoubles;

[ExcludeFromCodeCoverage]
[Collection("Sequential")]
public sealed class FeatureAccessorTests : IDisposable {

    private readonly ProcessSpy _processSpy = new();
    private readonly IServiceCollection _serviceCollection = Substitute.For<ServiceCollection>();

    public FeatureAccessorTests() {
        _serviceCollection.AddSingleton(_processSpy);
    }

    private FeatureAccessor CreateFactory() {
        var builder = new FeatureAccessorBuilder(_serviceCollection);
        builder.TryAddProvider<FakeProvider>();
        var accessor = builder.Build(_serviceCollection.BuildServiceProvider());
        _processSpy.ClearCalls();
        return accessor;
    }

    public void Dispose() {
        FeatureAccessor.Features.Clear();
        FeatureAccessor.StaticFlags.Clear();
    }

    [Fact]
    public void For_ExistingStaticFeature_ReturnsFlag_WithoutCallingProvider() {
        // Arrange
        var factory = CreateFactory();
        const string name = "Feature1";

        // Act
        var result = factory.For(name);

        // Assert
        result.IsEnabled.Should().BeTrue();
        _processSpy.GetCalls().Should().BeEmpty();
    }

    [Fact]
    public void For_ExistingScopedFeature_CallsProviderOnlyOnce_And_ReturnsFlag() {
        // Arrange
        var factory = CreateFactory();
        const string name = "Feature2";
        var previous = factory.For(name);

        // Act
        var result = factory.For(name);

        // Assert
        result.IsEnabled.Should().Be(previous.IsEnabled);
        _processSpy.GetCalls().Should().BeEquivalentTo("Constructor", "GetByNameOrDefault(Feature2)", "Dispose");
    }

    [Fact]
    public void For_ExistingTransientFeature_CallsProvider_And_ReturnsFlag() {
        // Arrange
        var factory = CreateFactory();
        const string name = "Feature3";

        // Act
        var result = factory.For(name);

        // Assert
        result.IsEnabled.Should().BeTrue();
        _processSpy.GetCalls().Should().BeEquivalentTo("Constructor", "GetByNameOrDefault(Feature3)", "Dispose");
    }

    [Fact]
    public void For_NonRegisteredFeature_ReturnsNullFlag() {
        // Arrange
        var factory = CreateFactory();
        const string name = "Invalid";

        // Act
        var result = factory.For(name);

        // Assert
        result.IsEnabled.Should().BeFalse();
        _processSpy.GetCalls().Should().BeEmpty();
    }

    [Fact]
    public void For_RemovedTransientFeature_ReturnsNullFlag() {
        // Arrange
        var factory = CreateFactory();
        const string name = "Feature8";

        // Act
        var result = factory.For(name);

        // Assert
        result.IsEnabled.Should().BeFalse();
        _processSpy.GetCalls().Should().BeEquivalentTo("Constructor", "GetByNameOrDefault(Feature8)", "Dispose");
    }
}