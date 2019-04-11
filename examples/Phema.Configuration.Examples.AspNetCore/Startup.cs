using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Phema.Configuration.Examples.AspNetCore
{
	public class Startup : IStartup
	{
		private readonly WebConfiguration configuration;

		public Startup(IOptions<WebConfiguration> configuration)
		{
			this.configuration = configuration.Value;
		}

		public IServiceProvider ConfigureServices(IServiceCollection services)
		{
			return services.BuildServiceProvider();
		}

		public void Configure(IApplicationBuilder app)
		{
			app.UseRouting(routes =>
				routes.MapGet("/", async context => 
					await context.Response.WriteAsync($"Hello from {configuration.App}!")));
		}
	}
}