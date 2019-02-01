using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace Phema.Configuration.Yaml
{
	public static class YamlConfigurationExtensions
	{
		public static IConfigurationBuilder AddYamlFile(
			this IConfigurationBuilder builder,
			string path,
			IFileProvider provider = null,
			bool optional = false,
			bool reloadOnChange = false)
		{
			builder.Add(new YamlFileConfigurationSource
			{
				FileProvider = provider,
				Path = path,
				Optional = optional,
				ReloadOnChange = reloadOnChange
			});
			
			return builder;
		}
	}
}