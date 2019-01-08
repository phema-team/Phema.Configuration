using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
			var host = new WebHostBuilder()
				.UseConfiguration(
					new ConfigurationBuilder()
						.AddInMemoryCollection(new Dictionary<string, string>
						{
							["Name"] = "Sarah",
						})
						.Build())
				.UseConfiguration<RootConfiguration>()
				.ConfigureServices(services => services.Configure<Configuration>(c => c.Name = "John"))
				.Configure(app => {})
				.Build();

			var configuration = host.Services.GetRequiredService<IOptions<Configuration>>().Value;
			
			Assert.Equal("John", configuration.Name);
		}
		
		[Fact]
		public void ConfigureWithConfigurationOverrides()
		{
			var host = new WebHostBuilder()
				.ConfigureServices(services => services.Configure<Configuration>(c => c.Name = "John"))
				.UseConfiguration(
					new ConfigurationBuilder()
						.AddInMemoryCollection(new Dictionary<string, string>
						{
							["Name"] = "Sarah",
						})
						.Build())
				.UseConfiguration<RootConfiguration>()
				.Configure(app => {})
				.Build();

			var configuration = host.Services.GetRequiredService<IOptions<Configuration>>().Value;
			
			Assert.Equal("John", configuration.Name);
		}
	}
}