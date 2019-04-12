using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Xunit;

namespace Phema.Configuration.Tests
{
	[Configuration]
	public class RootConfiguration
	{
		public ChildConfiguration Child { get; set; }

		[Configuration]
		public PropertyConfiguration Property { get; set; }

		public NoneConfiguration None { get; set; }

		public string Name { get; set; }
		
		public int Age { get; set; }
	}
	
	[Configuration]
	public class ChildConfiguration
	{
		public string Name { get; set; }  
	}
	
	public class PropertyConfiguration
	{
		public int Age { get; set; }
	}
	
	public class NoneConfiguration
	{
		public string Address { get; set; }
	}
	
	public class ConfigurationTests
	{
		[Fact]
		public void AddsConfiguration()
		{
			var host = new HostBuilder()
				.ConfigureAppConfiguration((c, b) =>
					b.AddInMemoryCollection(new Dictionary<string, string>
						{
							["Name"] = "RootName",
							["Age"] = "10",
							["Child:Name"] = "Test",
							["Property:Age"] = "12",
							["None:Address"] = "Address"
						}))
				.ConfigureServices((context, services) =>
					services.AddConfiguration<RootConfiguration>(context.Configuration))
				.Build();

			var root = host.Services.GetRequiredService<IOptions<RootConfiguration>>().Value;
			var child = host.Services.GetRequiredService<IOptions<ChildConfiguration>>().Value;
			var property = host.Services.GetRequiredService<IOptions<PropertyConfiguration>>().Value;
			var non = host.Services.GetService<IOptions<NoneConfiguration>>().Value;
			
			Assert.NotNull(root);
			Assert.NotNull(child);
			Assert.NotNull(property);
			Assert.NotNull(non);
			
			Assert.Equal("RootName", root.Name);
			Assert.Equal(10, root.Age);
			
			Assert.Equal("Test", child.Name);
			
			Assert.Equal(12, property.Age);
			
			Assert.NotNull(root.None);
			Assert.Equal("Address", root.None.Address);
			
			Assert.Null(non.Address);
		}
	}
}