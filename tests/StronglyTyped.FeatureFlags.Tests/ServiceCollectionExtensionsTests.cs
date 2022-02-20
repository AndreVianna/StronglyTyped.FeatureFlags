namespace StronglyTyped.FeatureFlags.Tests;

public class ServiceCollectionExtensionsTests {
    [Fact]
    public void AddFeatureFlags_Passes() {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton(new ProcessSpy());
        var configWasCalled = false;

        // Act
        services.AddFeatureFlags(opt => {
            opt.AddProvider<FakeProvider>();
            configWasCalled = true;
        });

        // Assert
        configWasCalled.Should().BeTrue();
    }
}