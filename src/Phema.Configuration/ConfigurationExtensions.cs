using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Phema.Configuration
{
	public static class ConfigurationExtensions
	{
		public static IWebHostBuilder AddConfiguration<TConfiguration>(this IWebHostBuilder builder)
			where TConfiguration : IConfiguration, new()
		{
			builder.ConfigureAppConfiguration((context, config) =>
			{
				builder.ConfigureServices(services =>
				{
					var configuration = new TConfiguration();
					context.Configuration.Bind(configuration);
					RegisterRecursive(services, configuration);
				});
			});

			return builder;
		}

		private static void RegisterRecursive(IServiceCollection services, IConfiguration configuration)
		{
			services.AddSingleton(configuration.GetType(), configuration);

			foreach (var innerConfiguration in GetInnerConfigurations(configuration))
			{
				RegisterRecursive(services, innerConfiguration);
			}
		}

		private static IEnumerable<IConfiguration> GetInnerConfigurations(IConfiguration configuration)
		{
			return configuration.GetType()
				.GetProperties(BindingFlags.Instance | BindingFlags.Public)
				.Where(x => x.CanRead)
				.Where(x => typeof(IConfiguration).IsAssignableFrom(x.PropertyType))
				.Select(x => (IConfiguration)x.GetValue(configuration));
		}
	}
}