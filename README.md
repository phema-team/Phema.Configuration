# Phema.Configuration

[![Build Status](https://cloud.drone.io/api/badges/phema-team/Phema.Configuration/status.svg)](https://cloud.drone.io/phema-team/Phema.Configuration) [![Nuget](https://img.shields.io/nuget/v/Phema.Configuration.svg)](https://www.nuget.org/packages/Phema.Configuration)

C# strongly typed `IConfiguration` wrapper

## Installation

```bash
$> dotnet add package Phema.Configuration
```

## Usage (or check for [examples](https://github.com/phema-team/Phema.Configuration/tree/master/examples))

```csharp
[Configuration]
public class RootConfiguration
{
  public InnerConfiguration Inner { get; set; }
}

[Configuration]
public class InnerConfiguration
{
}

// Add
var configuration = services.AddConfiguration<RootConfiguration>(Configuration);

// Get or inject with DI
var root = provider.GetRequiredService<IOptions<RootConfiguration>>().Value;
var inner = provider.GetRequiredService<IOptions<InnerConfiguration>>().Value;
```

## Tips

- To add configuration call `AddConfiguration<T>` on `IServiceCollection`
- Mark all your configuration parts with `ConfigurationAttribute` to inclute in configuration tree
- `ConfigurationAttribute` works both on type and property declaration
- All configuration parts adds to `IServiceCollection` recursively. You can resolve them in app calling `IServiceProvider` or inject using DI
