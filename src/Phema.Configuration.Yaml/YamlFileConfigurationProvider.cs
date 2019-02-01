using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using YamlDotNet.RepresentationModel;
using Microsoft.Extensions.Configuration;

namespace Phema.Configuration.Yaml
{
	internal class YamlFileConfigurationProvider : FileConfigurationProvider
	{
		public YamlFileConfigurationProvider(YamlFileConfigurationSource source) : base(source)
		{
		}

		public override void Load(Stream stream)
		{
			var yaml = new YamlStream();
			
			yaml.Load(new StreamReader(stream, true));
			
			if (yaml.Documents.Any())
			{
				Data = new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
				
				new YamlFileConfigurationVisitor(Data).Visit(yaml.Documents);
			}
		}
	}
}