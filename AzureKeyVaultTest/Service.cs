using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureKeyVaultTest;

public class Service : IHostedService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<Service> _logger;
    public Service(
        ILogger<Service> logger,
        IConfiguration configuration
        )

    {
        _logger = logger;
        _configuration = configuration;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("App started");
        await Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("App stopped");
        await Task.CompletedTask;
    }
}
