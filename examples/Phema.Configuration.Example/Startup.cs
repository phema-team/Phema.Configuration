using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Phema.Configuration.Example
{
	public class Startup
	{
		private readonly WebConfiguration configuration;

		public Startup(IOptions<WebConfiguration> configuration)
		{
			this.configuration = configuration.Value;
		}

		public void ConfigureServices(IServiceCollection services)
		{
		}

		public void Configure(IApplicationBuilder app)
		{
			app.UseRouting(routes =>
				routes.MapGet("/", async context => 
					await context.Response.WriteAsync($"Hello from {configuration.App}!")));
		}
	}
}