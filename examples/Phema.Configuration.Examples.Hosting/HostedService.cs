using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Phema.Configuration.Examples.Hosting
{
	public class HostedService : IHostedService
	{
		private readonly HostConfiguration configuration;

		public HostedService(IOptions<HostConfiguration> configuration)
		{
			this.configuration = configuration.Value;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			return Console.Out.WriteLineAsync($"Hello from {configuration.App}!");
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}
	}
}