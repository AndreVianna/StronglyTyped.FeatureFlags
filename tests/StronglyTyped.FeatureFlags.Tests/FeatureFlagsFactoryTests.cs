namespace StronglyTyped.FeatureFlags.Tests;
using TestDoubles;

[ExcludeFromCodeCoverage]
[Collection("Sequential")]
public class FeatureFlagsFactoryTests {

    private readonly ProcessSpy _processSpy = new();
    private readonly IServiceCollection _serviceCollection = Substitute.For<ServiceCollection>();

    public FeatureFlagsFactoryTests() {
        _serviceCollection.AddSingleton(_processSpy);
    }

    private FeatureFlagsFactory CreateFactory() {
        var builder = new FeatureFlagsFactoryBuilder(_serviceCollection);
        builder.TryAddProvider<FakeProvider>();
        var factory = builder.Build(_serviceCollection.BuildServiceProvider());
        _processSpy.ClearCalls();
        return factory;
    }

    private static void CleanUp() {
        FeatureFlagsFactory.Features.Clear();
        FeatureFlagsFactory.StaticFlags.Clear();
    }

    [Fact]
    public void For_ExistingStaticFeature_ReturnsFlag_WithoutCallingProvider() {
        try {
            // Arrange
            var factory = CreateFactory();
            const string name = "Feature1";

            // Act
            var result = factory.For(name);

            // Assert
            result.Should().NotBeOfType<Flag>();
            result.IsEnabled.Should().BeTrue();
            _processSpy.GetCalls().Should().BeEmpty();
        }
        finally {
            // Annihilate
            CleanUp();
        }
    }

    [Fact]
    public void For_ExistingScopedFeature_CallsProviderOnce_And_ReturnsFlag() {
        try {
            // Arrange
            var factory = CreateFactory();
            const string name = "Feature2";

            // Act
            var result = factory.For(name);

            // Assert
            result.Should().NotBeOfType<Flag>();
            result.IsEnabled.Should().BeTrue();
            _processSpy.GetCalls().Should().BeEmpty();
        }
        finally {
            // Annihilate
            CleanUp();
        }
    }

    [Fact]
    public void For_ExistingTransientFeature_CallsProvider_And_ReturnsFlag() {
        try {
            // Arrange
            var factory = CreateFactory();
            const string name = "Feature3";

            // Act
            var result = factory.For(name);

            // Assert
            result.Should().NotBeOfType<Flag>();
            result.IsEnabled.Should().BeTrue();
            _processSpy.GetCalls().Should().BeEquivalentTo("Constructor", "GetByNameOrDefault(Feature3)", "Dispose");
        }
        finally {
            // Annihilate
            CleanUp();
        }
    }

    [Fact]
    public void For_NonRegisteredFeature_ReturnsNullFlag() {
        try {
            // Arrange
            var factory = CreateFactory();
            const string name = "Invalid";

            // Act
            var result = factory.For(name);

            // Assert
            result.Should().BeOfType<NullFlag>();
            result.IsEnabled.Should().BeFalse();
            _processSpy.GetCalls().Should().BeEmpty();
        }
        finally {
            // Annihilate
            CleanUp();
        }
    }

    [Fact]
    public void For_RemovedTransientFeature_ReturnsNullFlag() {
        try {
            // Arrange
            var factory = CreateFactory();
            const string name = "Feature8";

            // Act
            var result = factory.For(name);

            // Assert
            result.Should().BeOfType<NullFlag>();
            result.IsEnabled.Should().BeFalse();
            _processSpy.GetCalls().Should().BeEquivalentTo("Constructor", "GetByNameOrDefault(Feature8)", "Dispose");
        }
        finally {
            // Annihilate
            CleanUp();
        }
    }


    [Fact]
    public void For_RemovedScopedFeature_ReturnsNullFlag() {
        try {
            // Arrange
            var factory = CreateFactory();
            const string name = "Feature7";

            // Act
            var result = factory.For(name);

            // Assert
            result.Should().BeOfType<NullFlag>();
            result.IsEnabled.Should().BeFalse();
            _processSpy.GetCalls().Should().BeEmpty();
        }
        finally {
            // Annihilate
            CleanUp();
        }
    }
}