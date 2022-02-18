nuget delete StronglyTyped.FeatureFlags.Providers.Configuration 0.1.0-rc1 -source c:\nuget\packages -noprompt
nuget delete StronglyTyped.FeatureFlags 0.1.0-rc1 -source c:\nuget\packages -noprompt
nuget delete StronglyTyped.FeatureFlags.Abstractions 0.1.0-rc1 -source c:\nuget\packages -noprompt
nuget delete StronglyTyped.FeatureFlags.Generator 0.1.0-rc1 -source c:\nuget\packages -noprompt


dotnet build gen\StronglyTyped.FeatureFlags.Generator\StronglyTyped.FeatureFlags.Generator.csproj -c Release
dotnet pack gen\StronglyTyped.FeatureFlags.Generator\StronglyTyped.FeatureFlags.Generator.csproj -c Release
nuget add gen\StronglyTyped.FeatureFlags.Generator\bin\Release\StronglyTyped.FeatureFlags.Generator.0.1.0-rc1.nupkg -source c:\nuget\packages

dotnet build src\StronglyTyped.FeatureFlags.Abstractions\StronglyTyped.FeatureFlags.Abstractions.csproj -c Release
dotnet pack src\StronglyTyped.FeatureFlags.Abstractions\StronglyTyped.FeatureFlags.Abstractions.csproj -c Release
nuget add src\StronglyTyped.FeatureFlags.Abstractions\bin\Release\StronglyTyped.FeatureFlags.Abstractions.0.1.0-rc1.nupkg -source c:\nuget\packages

dotnet build src\StronglyTyped.FeatureFlags\StronglyTyped.FeatureFlags.csproj -c Release
dotnet pack src\StronglyTyped.FeatureFlags\StronglyTyped.FeatureFlags.csproj -c Release
nuget add src\StronglyTyped.FeatureFlags\bin\Release\StronglyTyped.FeatureFlags.0.1.0-rc1.nupkg -source c:\nuget\packages

dotnet build src\StronglyTyped.FeatureFlags.Providers.Configuration\StronglyTyped.FeatureFlags.Providers.Configuration.csproj -c Release
dotnet pack src\StronglyTyped.FeatureFlags.Providers.Configuration\StronglyTyped.FeatureFlags.Providers.Configuration.csproj -c Release
nuget add src\StronglyTyped.FeatureFlags.Providers.Configuration\bin\Release\StronglyTyped.FeatureFlags.Providers.Configuration.0.1.0-rc1.nupkg -source c:\nuget\packages
