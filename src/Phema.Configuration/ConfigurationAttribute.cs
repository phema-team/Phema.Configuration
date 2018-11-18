using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Phema.Configuration.Tests")]

namespace Phema.Configuration
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
	public class ConfigurationAttribute : Attribute
	{
	}
}