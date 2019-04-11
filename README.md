# Phema.Configuration

[![Build Status](https://cloud.drone.io/api/badges/phema-team/Phema.Configuration/status.svg)](https://cloud.drone.io/phema-team/Phema.Configuration) [![Nuget](https://img.shields.io/nuget/v/Phema.Configuration.svg)](https://www.nuget.org/packages/Phema.Configuration)

C# strongly typed `IConfiguration` wrapper

**Does not work** with `ASP.NET Core 3.0` generic hosts (`ConfigureWebHost`, `ConfigureWebHostDefaults`), `WebHost` works fine

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
WebHost.CreateDefaultBuilder<Startup>(args)
  .UseConfiguration<RootConfiguration>();

// Or
Host.CreateDefaultBuilder(args)
  .UseConfiguration<RootConfiguration>();

// Get or inject with DI
var root = provider.GetRequiredService<IOptions<RootConfiguration>>().Value;
var inner = provider.GetRequiredService<IOptions<InnerConfiguration>>().Value;

Assert.Equal(root.Inner, inner);
```

- Maps `IConfiguration` from `Microsoft.Extensions.Configuration` to strongly typed configuration
- All configuration parts adds to IServiceCollection recursively, so you can resolve them in app calling `IServiceProvider` or inject using DI
- You can add configuration by calling `UseConfiguration<T>` on `IWebHostBuilder`
- You need to mark all your configuration parts by `ConfigurationAttribute` in this package
- You can add `ConfigurationAttribute` to class or property if class is not avaliable to modification