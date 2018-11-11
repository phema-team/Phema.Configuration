using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Phema.Configuration.Tests
{
	public class RootConfiguration : IConfiguration
	{
		public ChildConfiguration Child { get; private set; }

		public string Name { get; private set; }
		
		public int Age { get; private set; }
	}
	
	public class ChildConfiguration : IConfiguration
	{
		public string Name { get; private set; }  
	}
	
	public class CommonTests
	{
		[Fact]
		public void AddsConfiguration()
		{
			var configuration = new ConfigurationBuilder()
				.AddInMemoryCollection(new Dictionary<string, string>
				{
					["Name"] = "RootName",
					["Age"] = "10",
					["Child:Name"] = "Test"
				})
				.Build();
			
			var host = new WebHostBuilder()
				.UseConfiguration(configuration)
				.UseConfiguration<RootConfiguration>()
				.Configure(app => {})
				.Build();

			var root = host.Services.GetRequiredService<RootConfiguration>();
			var child = host.Services.GetRequiredService<ChildConfiguration>();
			
			Assert.Equal("RootName", root.Name);
			Assert.Equal(10, root.Age);
			Assert.Equal(child, root.Child);
			Assert.Equal("Test", child.Name);
		}
	}
}