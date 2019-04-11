using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Phema.Configuration.Examples.Hosting
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			await Host.CreateDefaultBuilder(args)
				.UseConfiguration<HostConfiguration>()
				.ConfigureServices(s => s.AddHostedService<HostedService>())
				.RunConsoleAsync();
		}
	}
}