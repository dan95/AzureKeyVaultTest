using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

var hostBuilder = new HostBuilder();

hostBuilder.ConfigureLogging(x =>
{
    x.AddSimpleConsole(c =>
    {
        c.TimestampFormat = "dd/MM/yyyy HH:mm:ss";
        c.ColorBehavior = LoggerColorBehavior.Enabled;
    });
});

hostBuilder.ConfigureAppConfiguration(x =>
{
    x.AddJsonFile("appsettings.json", false, reloadOnChange: true);

    var config = x.Build();

    x.AddAzureAppConfiguration(options =>
    {
        options.Connect(config.GetConnectionString("AppConfig"));
        options.ConfigureKeyVault(kv =>
        {
            kv.SetCredential(new ClientSecretCredential(
                tenantId: config.GetValue<string>("SecretConfiguration:TenantId"),
                clientId: config.GetValue<string>("SecretConfiguration:ClientId"),
                clientSecret: config.GetValue<string>("SecretConfiguration:Secret")
            ));
        });
    });
});

hostBuilder.ConfigureServices(x =>
{
    x.AddHostedService<Service>();
});


var app = hostBuilder.Build();

await app.RunAsync();

public class Service : IHostedService
{
    private IConfiguration _configuration;
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