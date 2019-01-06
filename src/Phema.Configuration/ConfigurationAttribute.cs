using System;

namespace Phema.Configuration
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
	public class ConfigurationAttribute : Attribute
	{
	}
}