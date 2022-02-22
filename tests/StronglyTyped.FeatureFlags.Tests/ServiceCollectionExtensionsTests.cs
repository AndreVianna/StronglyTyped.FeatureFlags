namespace StronglyTyped.FeatureFlags.Tests;
using TestDoubles;

[ExcludeFromCodeCoverage]
public class ServiceCollectionExtensionsTests {
    [Fact]
    public void AddFeatureFlags_RegisterProvider_AndCallsConfigAction() {
        // Arrange
        var processSpy = new ProcessSpy();
        var services = new ServiceCollection();
        services.AddSingleton(processSpy);
        var configWasCalled = false;

        // Act
        services.AddFeatureFlags(opt => {
            opt.AddProvider<FakeProvider>();
            configWasCalled = true;
        });

        // Assert
        configWasCalled.Should().BeTrue();
        processSpy.GetCalls().Should().BeEquivalentTo("Constructor", "Name", "Name", "GetAll", "Name", "Name", "Name", "Dispose");
    }
}