using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Phema.Configuration
{
	public static class ConfigurationExtensions
	{
		/// <summary>
		/// Add all options in configuration tree and return <see cref="TConfiguration"/>
		/// </summary>
		public static TConfiguration AddConfiguration<TConfiguration>(
			this IServiceCollection services,
			IConfiguration configuration,
			Action<BinderOptions> binder = null)
		{
			RegisterRecursive(services, configuration, binder, typeof(TConfiguration));

			return configuration.Get<TConfiguration>();
		}

		private static void RegisterRecursive(
			IServiceCollection services,
			IConfiguration configuration,
			Action<BinderOptions> binder,
			Type type)
		{
			RegisterConfigureOptions(services, configuration, binder, type);

			foreach (var (name, property) in GetInnerConfigurationProperties(type))
			{
				RegisterRecursive(services, configuration.GetSection(name), binder, property);
			}
		}

		private static void RegisterConfigureOptions(
			IServiceCollection services,
			IConfiguration configuration,
			Action<BinderOptions> binder,
			Type type)
		{
			var serviceType = typeof(IConfigureOptions<>).MakeGenericType(type);
			var optionsType = typeof(ConfigurationConfigureOptions<>).MakeGenericType(type);

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