using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Phema.Configuration
{
	public static class ConfigurationExtensions
	{
		/// <summary>
		///   Add all options in configuration tree and return <see cref="TConfiguration" />
		/// </summary>
		public static TConfiguration AddConfiguration<TConfiguration>(
			this IServiceCollection services,
			IConfiguration configuration,
			Action<BinderOptions> binder = null)
		{
			AddConfigurationRecursive(services, configuration, binder, typeof(TConfiguration));

			return configuration.Get<TConfiguration>();
		}

		private static void AddConfigurationRecursive(
			IServiceCollection services,
			IConfiguration configuration,
			Action<BinderOptions> binder,
			Type type)
		{
			AddConfigureOptions(services, configuration, binder, type);

			foreach (var (name, property) in GetInnerConfigurationProperties(type))
				AddConfigurationRecursive(services, configuration.GetSection(name), binder, property);
		}

		private static void AddConfigureOptions(
			IServiceCollection services,
			IConfiguration configuration,
			Action<BinderOptions> binder,
			Type property)
		{
			var serviceType = typeof(IConfigureOptions<>).MakeGenericType(property);
			var optionsType = typeof(ConfigurationConfigureOptions<>).MakeGenericType(property);

			var configureOptions = Activator.CreateInstance(optionsType, configuration, binder);

			services.TryAddSingleton(serviceType, sp => configureOptions);
		}

		private static IEnumerable<(string, Type)> GetInnerConfigurationProperties(IReflect type)
		{
			return type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
				.Where(property => property.CanRead)
				.Where(property => property.IsDefined(typeof(ConfigurationAttribute))
					|| property.PropertyType.IsDefined(typeof(ConfigurationAttribute)))
				.Select(property => (property.Name, property.PropertyType));
		}
	}
}