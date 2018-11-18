# Phema.Configuration
C# lightweight configuration map wrapper

- [x] Extensions
- [x] Tests

# Usage
```csharp
WebHost.CreateDefaultBuilder<Startup>()
  .UseConfiguration<TestConfiguration>()
  .Build();
```

- Maps `IConfiguration` from `Microsoft.Extensions.Configuration` to strongly typed configuration
- You can add configuration by calling `UseConfiguration<T>` on `IWebHostBuilder`
- You need to inherit all your configuration parts from empty `IConfiguration` in this package
- You can override property names using `DataMember` and `DataContract` attribute
- All configuration parts adds to IServiceCollection recursively, so you can resolve them in app calling `IServiceProvider`
