using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Phema.Configuration.Example
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.UseConfiguration<WebConfiguration>()
				.ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
	}
}