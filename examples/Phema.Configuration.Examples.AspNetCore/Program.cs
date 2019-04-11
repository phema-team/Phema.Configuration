using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Phema.Configuration.Examples.AspNetCore
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IWebHostBuilder CreateHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder<Startup>(args)
				.UseConfiguration<WebConfiguration>();
	}
}