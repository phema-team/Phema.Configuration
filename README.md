# Phema.Configuration

[![Build Status](https://cloud.drone.io/api/badges/phema-team/Phema.Configuration/status.svg)](https://cloud.drone.io/phema-team/Phema.Configuration) [![Nuget](https://img.shields.io/nuget/v/Phema.Configuration.svg)](https://www.nuget.org/packages/Phema.Configuration)

C# strongly typed `IConfiguration` wrapper

## Usage

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
Host.CreateDefaultBuilder(args)
  .UseConfiguration<RootConfiguration>()
  .ConfigureWebHost(webBuilder => webBuilder.UseStartup<Startup>())
  .Build();

// Get
var root = provider.GetRequiredService<IOptions<RootConfiguration>>().Value;
var inner = provider.GetRequiredService<IOptions<InnerConfiguration>>().Value;

Assert.Equal(root.Inner, inner);
```

- Maps `IConfiguration` from `Microsoft.Extensions.Configuration` to strongly typed configuration
- All configuration parts adds to IServiceCollection recursively, so you can resolve them in app calling `IServiceProvider` or inject using DI
- You can add configuration by calling `UseConfiguration<T>` on `IHostBuilder`
- You need to mark all your configuration parts by `ConfigurationAttribute` in this package
- You can add `ConfigurationAttribute` to class or property if class is not avaliable to modification