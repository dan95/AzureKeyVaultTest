using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace AzureKeyVaultTestCommon
{
    public class Service : IHostedService
    {
        private readonly ILogger<Service> _logger;
        private readonly IConfiguration _configuration;

        public Service(
            ILogger<Service> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(
            "{Class} | {Method} | {Config}",
            nameof(Service),
            nameof(StartAsync),
            _configuration.AsEnumerable().Select(x => new { x.Key, x.Value })
            );

            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Class} | {Method} | App stopped", nameof(Service), nameof(StopAsync));
            await Task.CompletedTask;
        }
    }
}
