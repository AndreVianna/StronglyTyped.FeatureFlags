nuget delete StronglyTyped.FeatureFlags.Abstractions 0.1.0-rc1 -source c:\nuget\packages -noprompt


dotnet build -c Release
dotnet pack -c Release
nuget add bin\Release\StronglyTyped.FeatureFlags.Abstractions.0.1.0-rc1.nupkg -source c:\nuget\packages
