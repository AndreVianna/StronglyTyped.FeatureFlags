using Microsoft.AspNetCore.Mvc;

using StronglyTyped.FeatureFlags;
using StronglyTyped.FeatureFlags.Abstractions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFeatureFlags(opt => opt.AddProvider<ConfigurationFeatureFlagsProvider>());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/hello", ([FromServices] IFeatureFlagsFactory flags) =>
        flags.For("SaluteUniverse").IsEnabled
            ? "Hello Universe!"
            : "Hello world!")
.WithName("Salute");

app.Run();
