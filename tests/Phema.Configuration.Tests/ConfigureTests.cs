using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Xunit;

namespace Phema.Configuration.Tests
{
	public class ConfigureTests
	{
		public class Configuration
		{
			public string Name { get; set; }
		}

		[Fact]
		public void ConfigurationWithConfigureOverrides()
		{
			var host = new HostBuilder()
				.ConfigureAppConfiguration((c, b) =>
					b.AddInMemoryCollection(new Dictionary<string, string>
					{
						["Name"] = "Sarah",
					}))
				.UseConfiguration<RootConfiguration>()
				.ConfigureServices(services => services.Configure<Configuration>(c => c.Name = "John"))
				.Build();

			var configuration = host.Services.GetRequiredService<IOptions<Configuration>>().Value;

			Assert.Equal("John", configuration.Name);
		}

		[Fact]
		public void ConfigureWithConfigurationOverrides()
		{
			var host = new HostBuilder()
				.ConfigureServices(services => services.Configure<Configuration>(c => c.Name = "John"))
				.ConfigureAppConfiguration((c, b) =>
					b.AddInMemoryCollection(new Dictionary<string, string>
					{
						["Name"] = "Sarah",
					}))
				.UseConfiguration<RootConfiguration>()
				.Build();

			var configuration = host.Services.GetRequiredService<IOptions<Configuration>>().Value;

			Assert.Equal("John", configuration.Name);
		}
	}
}