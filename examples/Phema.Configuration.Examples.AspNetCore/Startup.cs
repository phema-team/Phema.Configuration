using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Phema.Configuration.Examples.AspNetCore
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			var configuration = services.AddConfiguration<WebConfiguration>(Configuration);

			// Use for setup connection strings, credentials, etc.
		}

		public void Configure(IApplicationBuilder app)
		{
			// Resolve once
			// var configuration = app.ApplicationServices.GetRequiredService<IOptions<WebConfiguration>>().Value;

			app.UseRouting(routes =>
				routes.MapGet("/", async context =>
				{
					// Resolve each request
					var configuration = context.RequestServices.GetRequiredService<IOptions<WebConfiguration>>().Value;

					await context.Response.WriteAsync($"Hello from {configuration.App}!");
				}));
		}
	}
}