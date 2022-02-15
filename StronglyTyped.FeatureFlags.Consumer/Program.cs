// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using StronglyTyped.FeatureFlags;
using StronglyTyped.FeatureFlags.Abstractions;

var serviceProvider = CreateServiceProvider();

var flags = serviceProvider.GetRequiredService<IFeatureFlagsFactory>();
if (flags.For("SaluteUniverse").IsEnabled) Console.WriteLine("Hello Universe!");
else Console.WriteLine("Hello, World!");

static IServiceProvider CreateServiceProvider() {
    var services = new ServiceCollection();

    var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .Build();

    services.AddSingleton<IConfiguration>(_ => config);
    services.AddFeatureFlags(opt => opt.AddProvider<ConfigurationFeatureFlagsProvider>());

    return services.BuildServiceProvider(true).CreateScope().ServiceProvider;
}