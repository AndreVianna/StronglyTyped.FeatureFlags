namespace StronglyTyped.FeatureFlags.Tests;
using TestDoubles;

[ExcludeFromCodeCoverage]
[Collection("Sequential")]
public sealed class ServiceCollectionExtensionsTests : IDisposable {
    public void Dispose() {
        FeatureReader.Features.Clear();
        FeatureReader.StaticFlags.Clear();
    }

    [Fact]
    public void AddFeatureFlags_RegisterProvider_AndCallsConfigAction() {
        // Arrange
        var processSpy = new ProcessSpy();
        var services = new ServiceCollection();
        services.AddSingleton(processSpy);
        var configWasCalled = false;

        // Act
        services.AddFeatureFlagsFrom<ServiceCollectionExtensionsTests>(opt => {
            opt.TryAddProvider<FakeProvider>();
            configWasCalled = true;
        });

        // Assert
        using var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        var provider = scope.ServiceProvider.GetService<IFeatureReader>();
        provider.Should().NotBeNull();

        var features = scope.ServiceProvider.GetRequiredService<ITestFeatures>();
        features.Should().NotBeNull();
        var subFeatures = scope.ServiceProvider.GetRequiredService<ITestSubFeatures>();
        subFeatures.Should().NotBeNull();

        configWasCalled.Should().BeTrue();
        processSpy.GetCalls().Should().BeEquivalentTo("Constructor", "GetAll", "Dispose");
        provider.Should().NotBeNull();
    }
}