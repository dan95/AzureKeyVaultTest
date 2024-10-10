using AzureKeyVaultRestApiTest;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

var hostBuilder = new HostBuilder();

hostBuilder.ConfigureAppConfiguration(x =>
{
    x.AddJsonFile("appsettings.json", false, reloadOnChange: true);
});

hostBuilder.ConfigureLogging(x =>
{
    x.AddSimpleConsole(c =>
    {
        c.TimestampFormat = "dd/MM/yyyy HH:mm:ss";
        c.ColorBehavior = LoggerColorBehavior.Enabled;
    });
});

hostBuilder.ConfigureServices((context, services) => {

    services.Configure<AzureApiSettings>(context.Configuration.GetSection(nameof(AzureApiSettings)));

    services.AddHttpClient<Service>();

    services.AddHostedService<Service>();
});

await hostBuilder.RunConsoleAsync();