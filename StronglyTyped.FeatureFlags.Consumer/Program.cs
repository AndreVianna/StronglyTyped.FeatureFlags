// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using StronglyTyped.FeatureFlags;
using StronglyTyped.FeatureFlags.Consumer;

var serviceProvider = CreateServiceProvider();

var flags = serviceProvider.GetRequiredService<IFeatureAccessor>();
if (flags.SaluteUniverse.IsEnabled) Console.WriteLine("Hello Universe!");
else Console.WriteLine("Hello, World!");

static IServiceProvider CreateServiceProvider() {
    var services = new ServiceCollection();

    var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .Build();

    services.AddSingleton<IConfiguration>(_ => config);
    services.AddFeatureFlags(opt => opt.AddProvider<ConfigurationFeatureProvider>());
    services.AddSingleton<IFeatureAccessor, FeatureAccessor>();

    return services.BuildServiceProvider(true).CreateScope().ServiceProvider;
}