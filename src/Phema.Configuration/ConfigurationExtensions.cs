using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Phema.Configuration
{
	public static class ConfigurationExtensions
	{
		public static IWebHostBuilder UseConfiguration<TConfiguration>(this IWebHostBuilder builder)
		{
			return builder.ConfigureAppConfiguration((context, config) =>
			{
				builder.ConfigureServices(services =>
					RegisterRecursive(services,
						context.Configuration.Get<TConfiguration>(
							options => options.BindNonPublicProperties = true)));
			});
		}

		private static void RegisterRecursive(IServiceCollection services, object configuration)
		{
			RegisterOptions(services, configuration);

			foreach (var innerConfiguration in GetInnerConfigurations(configuration))
			{
				RegisterRecursive(services, innerConfiguration);
			}
		}

		private static void RegisterOptions(IServiceCollection services, object configuration)
		{
			var type = configuration.GetType();

			var serviceType = typeof(IOptions<>).MakeGenericType(type);

			var instance = Activator.CreateInstance(
				typeof(OptionsWrapper<>).MakeGenericType(type),
				configuration);

			services.AddSingleton(serviceType, instance);
		}

		internal static IEnumerable<object> GetInnerConfigurations(object configuration)
		{
			return configuration.GetType()
				.GetProperties(BindingFlags.Instance | BindingFlags.Public)
				.Where(property => property.CanRead)
				.Where(property => property.IsDefined(typeof(ConfigurationAttribute))
					|| property.PropertyType.IsDefined(typeof(ConfigurationAttribute)))
				.Select(property => property.GetValue(configuration))
				.Where(x => x != null);
		}
	}
}