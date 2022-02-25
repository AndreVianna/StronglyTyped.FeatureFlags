using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using StronglyTyped.FeatureFlags;
using StronglyTyped.FeatureFlags.TestConsumer;
using StronglyTyped.FeatureFlags.Providers.Configuration;

var serviceProvider = CreateServiceProvider();

var flags = serviceProvider.GetRequiredService<IFeatures>();
if (flags.SaluteUniverse.IsEnabled) Console.WriteLine("Hello Universe!");
else Console.WriteLine("Hello, World!");

static IServiceProvider CreateServiceProvider() {
    var services = new ServiceCollection();

    var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .Build();

    services.AddSingleton<IConfiguration>(_ => config);
    services.AddFeatureFlags(opt => opt.TryAddProvider<ConfigurationFeatureProvider>());
    services.AddScoped<IFeatures, Features>();

    return services.BuildServiceProvider(true).CreateScope().ServiceProvider;
}