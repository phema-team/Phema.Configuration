using Microsoft.Extensions.Configuration;

namespace Phema.Configuration.Yaml
{
	internal class YamlFileConfigurationSource : FileConfigurationSource
	{
		public override IConfigurationProvider Build(IConfigurationBuilder builder)
		{
			FileProvider = FileProvider ?? builder.GetFileProvider();
			
			return new YamlFileConfigurationProvider(this);
		}
	}
}