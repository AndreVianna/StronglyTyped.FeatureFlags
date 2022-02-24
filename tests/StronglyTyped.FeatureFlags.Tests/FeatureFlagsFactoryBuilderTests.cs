using StronglyTyped.FeatureFlags.Tests.TestDoubles;

namespace StronglyTyped.FeatureFlags.Tests;

[ExcludeFromCodeCoverage]
[Collection("Sequential")]
public sealed class FeatureFlagsFactoryBuilderTests : IDisposable {

    private readonly ProcessSpy _processSpy = new();
    private readonly IServiceCollection _serviceCollection = Substitute.For<ServiceCollection>();

    public FeatureFlagsFactoryBuilderTests() {
        _serviceCollection.AddSingleton(_processSpy);
    }

    private FeatureFlagsFactoryBuilder CreateBuilder() {
        var builder = new FeatureFlagsFactoryBuilder(_serviceCollection);
        _processSpy.ClearCalls();
        return builder;
    }

    public void Dispose() {
        FeatureFlagsFactory.Features.Clear();
        FeatureFlagsFactory.StaticFlags.Clear();
    }

    [Fact]
    public void AddProvider_ForNewProvider_RegistersTheProvider() {
        // Arrange
        var builder = CreateBuilder();

        // Act
        builder.TryAddProvider<FakeProvider>();

        // Assert
        _processSpy.GetCalls().Should().BeEquivalentTo("Constructor", "GetAll", "Dispose");
    }

    [Fact]
    public void AddProvider_ForNewProvider_WithConstructor_RegistersTheProvider() {
        // Arrange
        var factory = CreateBuilder();

        // Act
        factory.TryAddProvider(_ => new OtherFakeProvider(_processSpy));

        // Assert
        _processSpy.GetCalls().Should().BeEquivalentTo("Constructor", "GetAll", "Dispose");
    }

    [Fact]
    public void AddProvider_ForRegisteredProvider_IgnoresDuplicatedProvider() {
        // Arrange
        var factory = CreateBuilder();
        factory.TryAddProvider<FakeProvider>();
        _processSpy.ClearCalls();

        // Act
        factory.TryAddProvider<FakeProvider>();

        // Assert
        _processSpy.GetCalls().Should().BeEmpty();
    }

    [Fact]
    public void AddProvider_ForProviderWithDuplicatedFeature_Throws() {
        // Arrange
        var factory = CreateBuilder();
        factory.TryAddProvider<FakeProvider>();
        _processSpy.ClearCalls();

        // Act
        var action = [ExcludeFromCodeCoverage] () => factory.TryAddProvider<FakeProviderWithDuplicatedFeature>();

        // Assert
        action.Should().Throw<InvalidOperationException>()
            .WithMessage("Duplicated feature(s) found while registering the  'FakeProviderWithDuplicatedFeature' provider:\r\n\t'Feature2' found in 'FakeProvider' provider.\r\n");
        _processSpy.GetCalls().Should().BeEquivalentTo("Constructor", "GetAll", "Dispose");
    }
}