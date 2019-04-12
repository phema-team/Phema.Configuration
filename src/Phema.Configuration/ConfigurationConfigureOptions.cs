using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Phema.Configuration
{
	public class ConfigurationConfigureOptions<TOptions> : IConfigureOptions<TOptions>
		where TOptions : class
	{
		private readonly Action<BinderOptions> binder;
		private readonly IConfiguration configuration;

		public ConfigurationConfigureOptions(IConfiguration configuration, Action<BinderOptions> binder)
		{
			this.configuration = configuration;
			this.binder = binder;
		}

		public void Configure(TOptions options)
		{
			configuration.Bind(options, binder);
		}
	}
}