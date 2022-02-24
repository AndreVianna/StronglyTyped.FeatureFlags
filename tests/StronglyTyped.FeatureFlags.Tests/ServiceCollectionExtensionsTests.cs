namespace StronglyTyped.FeatureFlags.Tests;
using TestDoubles;

[ExcludeFromCodeCoverage]
[Collection("Sequential")]
public sealed class ServiceCollectionExtensionsTests : IDisposable {
    public void Dispose() {
        FeatureFlagsFactory.Features.Clear();
        FeatureFlagsFactory.StaticFlags.Clear();
    }

    [Fact]
    public void AddFeatureFlags_RegisterProvider_AndCallsConfigAction() {
        // Arrange
        var processSpy = new ProcessSpy();
        var services = new ServiceCollection();
        services.AddSingleton(processSpy);
        var configWasCalled = false;

        // Act
        services.AddFeatureFlags(opt => {
            opt.TryAddProvider<FakeProvider>();
            configWasCalled = true;
        });

        using var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        var provider = scope.ServiceProvider.GetRequiredService<IFlagsFactory>();

        // Assert
        configWasCalled.Should().BeTrue();
        processSpy.GetCalls().Should().BeEquivalentTo("Constructor", "GetAll", "Dispose");
        provider.Should().NotBeNull();
    }
}