using System.Linq;
using System.Collections.Generic;

using YamlDotNet.RepresentationModel;
using Microsoft.Extensions.Configuration;

namespace Phema.Configuration.Yaml
{
	internal class YamlFileConfigurationVisitor
	{
		private static string[] Whitespaces { get; } = { "~", "null", "Null", "NULL" };
		
		private readonly IDictionary<string, string> data;
		private readonly Stack<string> path;
		
		private string CombinedPath => ConfigurationPath.Combine(path.Reverse());

		public YamlFileConfigurationVisitor(IDictionary<string, string> data)
		{
			this.data = data;
			path = new Stack<string>();
		}

		public void Visit(IList<YamlDocument> documents)
		{
			var mapping = (YamlMappingNode)documents[0].RootNode;
			VisitMapping(mapping);
		}
		
		private void VisitNode(string part, YamlNode node)
		{
			switch (node)
			{
				case YamlScalarNode scalarNode:
					VisitScalar(part, scalarNode);
					break;
				case YamlMappingNode mappingNode:
					VisitMapping(part, mappingNode);
					break;
				case YamlSequenceNode sequenceNode:
					VisitSequence(part, sequenceNode);
					break;
			}
		}
		
		private void VisitScalar(string part, YamlScalarNode yamlValue)
		{
			path.Push(part);
			
			data[CombinedPath] = IsNullValue(yamlValue) ? null : yamlValue.Value;
			
			path.Pop();
		}
		
		
		private void VisitMapping(string part, YamlMappingNode yamlValue)
		{
			path.Push(part);

			VisitMapping(yamlValue);

			path.Pop();
		}
		
		private void VisitMapping(YamlMappingNode node)
		{
			foreach (var (key, value) in node.Children)
			{
				VisitPair(((YamlScalarNode)key, value));
			}
		}

		private void VisitSequence(string part, YamlSequenceNode yamlValue)
		{
			path.Push(part);

			VisitSequence(yamlValue);

			path.Pop();
		}

		private void VisitSequence(YamlSequenceNode node)
		{
			for (var index = 0; index < node.Children.Count; index++)
			{
				VisitNode(index.ToString(), node.Children[index]);
			}
		}

		private void VisitPair((YamlScalarNode, YamlNode) pair)
		{
			var (key, value) = pair;
			VisitNode(key.Value, value);
		}
		
		private static bool IsNullValue(YamlScalarNode yamlValue)
		{
			return yamlValue.Style == YamlDotNet.Core.ScalarStyle.Plain
				&& Whitespaces.Contains(yamlValue.Value);
		}
	}
}