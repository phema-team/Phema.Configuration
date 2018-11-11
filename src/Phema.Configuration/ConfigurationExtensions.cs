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
		public static IWebHostBuilder UseConfiguration<TConfiguration>(this IWebHostBuilder builder)
			where TConfiguration : IConfiguration
		{
			return builder.ConfigureAppConfiguration((context, config) =>
			{
				builder.ConfigureServices(
					services =>
						RegisterRecursive(services, context.Configuration.Get<TConfiguration>(
							options => 
								options.BindNonPublicProperties = true)));
			});
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
				.Select(x => (IConfiguration)x.GetValue(configuration))
				.Where(x => x != null);
		}
	}
}